using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.Services;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class BillingController(
    ILogger<BillingController> logger,
    IMapper mapper,
    IBillingService billingService,
    IBillingCrossDatabaseSyncStrategyProvider billingSyncStrategyProvider,
    EmrAppDefaultsStartupService emrDefaultsStartupService,
    IHPatientService patientService,
    IHRetainershipService retainershipService,
    IEmrAppDefaultsService emrAppDefaultsService,
    IReceiptAccountingPostingService receiptAccountingPostingService,
    ApplicationDbContext context,
    IConfiguration configuration)
    : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BillingVM>), 200)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var invoices = await billingService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BillingVM>>(invoices));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoices");
            AddModelError("Unable to retrieve invoices");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("{billNo}")]
    [ProducesResponseType(typeof(BillingVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByBillNo(string billNo)
    {
        try
        {
            var invoice = await billingService.GetByBillNoAsync(billNo);
            if (invoice is null)
            {
                return NotFound(billNo);
            }

            var details = await billingService.GetDetailsAsync(billNo);
            var vm = _mapper.Map<BillingVM>(invoice);
            vm.Details = _mapper.Map<List<BillingDetailVM>>(details);

            return Ok(vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoice {BillNo}", billNo);
            AddModelError("Unable to retrieve invoice");
            return BadRequest(ModelState);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(BillingVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] BillingVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var billing = _mapper.Map<Billing>(model);
            var details = _mapper.Map<List<BillingDetail>>(model.Details);

            var result = await billingService.CreateAsync(billing, details);
            var response = _mapper.Map<BillingVM>(result.Billing);
            response.Details = _mapper.Map<List<BillingDetailVM>>(result.Details);

            return CreatedAtAction(nameof(GetByBillNo), new { billNo = response.BillNo }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice");
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{billNo}")]
    [ProducesResponseType(typeof(BillingVM), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(string billNo, [FromBody] BillingVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await billingService.GetByBillNoAsync(billNo);
            if (existing is null)
            {
                return NotFound(billNo);
            }

            var billing = _mapper.Map<Billing>(model);
            var details = _mapper.Map<List<BillingDetail>>(model.Details);
            var result = await billingService.UpdateAsync(billNo, billing, details);

            var response = _mapper.Map<BillingVM>(result.Billing);
            response.Details = _mapper.Map<List<BillingDetailVM>>(result.Details);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating invoice {BillNo}", billNo);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{billNo}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string billNo)
    {
        try
        {
            var existing = await billingService.GetByBillNoAsync(billNo);
            if (existing is null)
            {
                return NotFound(billNo);
            }

            await billingService.DeleteAsync(billNo);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice {BillNo}", billNo);
            AddModelError("Unable to delete invoice");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("sync-status")]
    [ProducesResponseType(typeof(BillingSyncStatusVM), 200)]
    public IActionResult GetSyncStatus()
    {
        var status = billingSyncStrategyProvider.CurrentStatus;

        return Ok(new BillingSyncStatusVM
        {
            EffectiveMode = status.EffectiveMode,
            PrimaryDataSource = status.PrimaryDataSource,
            PrimaryDatabase = status.PrimaryDatabase,
            IncludedDatabases = status.IncludedDatabases.ToList(),
            SameInstanceDatabases = status.SameInstanceDatabases.ToList(),
            CrossInstanceDatabases = status.CrossInstanceDatabases.ToList(),
            Warnings = status.Warnings.ToList()
        });
    }

    [HttpGet("defaults-status")]
    [ProducesResponseType(typeof(BillingDefaultsStatusVM), 200)]
    public IActionResult GetDefaultsStatus()
    {
        return Ok(new BillingDefaultsStatusVM
        {
            Loaded = emrDefaultsStartupService.Loaded,
            Error = emrDefaultsStartupService.LastError,
            LastCheckedAtUtc = emrDefaultsStartupService.LastCheckedAtUtc
        });
    }

    [HttpGet("{billNo}/print-data")]
    [ProducesResponseType(typeof(InvoicePrintDataVM), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> GetPrintData(string billNo, [FromQuery] bool allowZeroBalance = false)
    {
        try
        {
            var billing = await billingService.GetByBillNoAsync(billNo);
            if (billing is null)
                return NotFound(billNo);

            // ── Balance guard: invoice only generated when balance is non-zero ──
            var amountBilled = billing.AmountBilled ?? 0;
            var debtBF       = billing.DebtBF       ?? 0;
            var discount     = billing.Discount     ?? 0;
            var tax          = (decimal)(billing.Tax ?? 0);
            var amountPaid   = billing.AmountPaid   ?? 0;
            var balance      = debtBF + amountBilled + tax - discount - amountPaid;

            if (balance == 0 && !allowZeroBalance)
            {
                AddModelError("Invoice cannot be generated: balance is zero.");
                return UnprocessableEntity(ModelState);
            }

            var details  = await billingService.GetDetailsAsync(billNo);
            var defaults = await emrAppDefaultsService.GetAsync();
            var patient  = await patientService.GetByIdAsync(billing.pNo);

            var currentBill = details.Sum(d => d.subTotal ?? (decimal)(d.Price * d.Qty));
            var vatPercent = (decimal)defaults.Taxes.Pcent;
            var taxableAmount = Math.Max(0m, currentBill - discount);
            var computedVat = Math.Round(taxableAmount * (vatPercent / 100m), 2, MidpointRounding.AwayFromZero);
            balance = debtBF + currentBill + computedVat - discount - amountPaid;

            // ── clientCat sourced from VwhRecord (consultId == billNo) ──
            var vwhRecord = await context.VwhRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.ConsultId == billNo);

            var clientCat = (vwhRecord?.ClientCat ?? string.Empty).Trim();

            // ── Payer address ──
            string payerName    = string.Empty;
            string payerAddress = string.Empty;
            string payerPhone   = string.Empty;

            var isPrivate = string.Equals(clientCat, defaults.ClientCategoryPrivate, StringComparison.OrdinalIgnoreCase);

            if (isPrivate)
            {
                if (patient != null)
                {
                    payerName = $"{patient.PSurName} {patient.PFirstname}".Trim();
                    payerAddress = patient.HomeAddress ?? string.Empty;
                    payerPhone = patient.PPhoneNo ?? string.Empty;
                }
            }
            else if (!string.IsNullOrWhiteSpace(billing.clientID))
            {
                var retainerships = await retainershipService.GetAllAsync();
                var retainership  = retainerships.FirstOrDefault(r =>
                    string.Equals(r.RetainCode, billing.clientID, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(r.RetainId,   billing.clientID, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(r.ClientCatId, billing.clientID, StringComparison.OrdinalIgnoreCase));

                if (retainership != null)
                {
                    payerName    = retainership.RetainName;
                    payerAddress = retainership.Address ?? string.Empty;
                    payerPhone   = retainership.PhoneNo ?? string.Empty;
                }
            }

            if (string.IsNullOrWhiteSpace(payerName) && patient != null)
            {
                payerName = $"{patient.PSurName} {patient.PFirstname}".Trim();
            }

            var patientName = patient != null
                ? $"{patient.PSurName} {patient.PFirstname}".Trim()
                : billing.pNo;

            var vm = new InvoicePrintDataVM
            {
                BillHead  = defaults.BillHead,
                BillHead2 = defaults.BillHead2,
                BillHead3 = defaults.BillHead3,
                BillHead4 = defaults.BillHead4,
                BillNo    = billing.billNO,
                BillDate  = billing.bDate.ToString("dd-MMM-yyyy"),
                TaxName   = defaults.Taxes.TaxName,
                TIN       = defaults.Taxes.TIN,
                TaxPcent  = defaults.Taxes.Pcent,
                PatientName  = patientName,
                PatientNo    = billing.pNo,
                ClientCat    = clientCat,
                PayerName    = payerName,
                PayerAddress = payerAddress,
                PayerPhone   = payerPhone,
                DebtBF       = debtBF,
                AmountBilled = currentBill,
                Discount     = discount,
                Tax          = computedVat,
                AmountPaid   = amountPaid,
                Balance      = balance,
                Details = details.Select((d, i) => new InvoicePrintDetailVM
                {
                    Sno      = d.SNO > 0 ? d.SNO : i + 1,
                    ItemName = d.drgName,
                    Price    = d.Price,
                    Qty      = d.Qty,
                    SubTotal = d.subTotal ?? (decimal)(d.Price * d.Qty),
                    Category = d.Category,
                    BillType = d.billType
                }).ToList()
            };

            return Ok(vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving print data for {BillNo}", billNo);
            AddModelError("Unable to retrieve print data");
            return BadRequest(ModelState);
        }
    }

    [HttpPatch("{billNo}/discount")]
    [ProducesResponseType(typeof(BillingVM), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateDiscount(string billNo, [FromBody] UpdateDiscountVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var billing = await context.Billings.FirstOrDefaultAsync(x => x.billNO == billNo);
            if (billing is null)
                return NotFound(billNo);

            if (model.Discount < 0)
            {
                AddModelError("Discount cannot be negative.");
                return BadRequest(ModelState);
            }

            if (model.Discount > (billing.AmountBilled ?? 0))
            {
                AddModelError("Discount cannot exceed the billed amount.");
                return BadRequest(ModelState);
            }

            billing.Discount = model.Discount;
            await context.SaveChangesAsync();

            return Ok(_mapper.Map<BillingVM>(billing));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating discount for {BillNo}", billNo);
            AddModelError("Unable to update discount");
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Save receipt data for a billing record.
    /// Writes to: Payments, PaymentDetails, and PaymentTypes.
    /// Only PRIVATE patients are eligible; supports deposit receipts before billing is raised.
    /// </summary>
    [HttpPost("{billNo}/receipt")]
    [ProducesResponseType(typeof(ReceiptSavedVM), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> SaveReceipt(string billNo, [FromBody] SaveReceiptVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var normalizedBillNo = billNo?.Trim();
            if (string.IsNullOrWhiteSpace(normalizedBillNo))
            {
                AddModelError("Bill number is required.");
                return BadRequest(ModelState);
            }

            var payType = (model.PayType ?? string.Empty).Trim();
            var isCashPayment = string.Equals(payType, "Cash", StringComparison.OrdinalIgnoreCase);
            if (!isCashPayment && string.IsNullOrWhiteSpace(model.AccountNo))
            {
                AddModelError("Bank Account is required for non-cash payment.");
                return BadRequest(ModelState);
            }

            if (!model.AmountToPay.HasValue || model.AmountToPay.Value <= 0)
            {
                AddModelError("Amount to Pay must be greater than zero.");
                return BadRequest(ModelState);
            }

            var billing = await context.Billings.FirstOrDefaultAsync(x => x.billNO == normalizedBillNo);

            var vwhRecord = await context.VwhRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.ConsultId == normalizedBillNo);

            var defaults = await emrAppDefaultsService.GetAsync();
            var clientCat = (vwhRecord?.ClientCat ?? string.Empty).Trim();

            if (!string.Equals(clientCat, defaults.ClientCategoryPrivate, StringComparison.OrdinalIgnoreCase))
            {
                AddModelError($"Receipts can only be issued to PRIVATE patients. This patient's category is '{clientCat}'.");
                return UnprocessableEntity(ModelState);
            }

            var now = DateTime.Now;
            var receiptNo = $"RCT-{normalizedBillNo}-{now:yyyyMMddHHmmss}";
            var collision = await context.Payments.AnyAsync(p => p.ReceiptNo == receiptNo);
            if (collision)
                receiptNo = $"RCT-{normalizedBillNo}-{now:yyyyMMddHHmmssfff}";

            var amountBilled = billing?.AmountBilled ?? 0;
            var debtBF = billing?.DebtBF ?? 0;
            var discount = billing?.Discount ?? 0;
            var tax = (decimal)(billing?.Tax ?? 0);
            var existingAmountPaid = billing?.AmountPaid ?? 0;
            var balance = debtBF + amountBilled + tax - discount - existingAmountPaid;
            var payAmount = model.AmountToPay.Value;

            var patient = !string.IsNullOrWhiteSpace(vwhRecord?.PNo)
                ? await patientService.GetByIdAsync(vwhRecord.PNo)
                : billing is not null
                    ? await patientService.GetByIdAsync(billing.pNo)
                    : null;

            var patientNo = billing?.pNo ?? vwhRecord?.PNo;
            if (string.IsNullOrWhiteSpace(patientNo))
            {
                AddModelError($"Unable to resolve patient number for bill '{normalizedBillNo}'.");
                return UnprocessableEntity(ModelState);
            }

            var patientName = patient is null
                ? (vwhRecord?.Fullname ?? string.Empty)
                : $"{patient.PSurName} {patient.PFirstname}".Trim();

            var receivedBy = string.IsNullOrWhiteSpace(model.ReceivedBy)
                ? GetCurrentUserId()
                : model.ReceivedBy;

            var accountNo = model.AccountNo?.Trim();
            if (string.IsNullOrWhiteSpace(accountNo))
            {
                var revType = await context.hRevenueTypes.AsNoTracking().FirstOrDefaultAsync();
                accountNo = revType?.RevType ?? "CASH";
            }

            var paymentFor = amountBilled == 0 && balance == 0
                ? $"Deposit for {normalizedBillNo}"
                : $"Bill {normalizedBillNo}";

            var payment = new Payment
            {
                ReceiptNo = receiptNo,
                ReceiptDate = now,
                billNO = normalizedBillNo,
                pNO = patientNo,
                clinicID = null,
                paymentFor = paymentFor,
                AmountBilled = amountBilled,
                AmountPaid = payAmount,
                AmountInWord = AmountToWords(payAmount),
                Receivedby = receivedBy,
                payType = payType,
                rTime = now,
                Remarks = model.Remarks,
                RetainCode = billing?.clientID ?? vwhRecord?.RetainCode ?? vwhRecord?.Coyname,
                ChequeNo = model.ChequeNo,
                ValueDate = model.ValueDate,
                BankCode = model.BankCode,
                ChequeDate = model.ValueDate,
                isPost = false,
                EntryDate = now,
                EntryTime = now,
                ClientName = patientName,
                AppName = "AestheticEMR",
                suppres = false
            };
            context.Payments.Add(payment);

            var details = await billingService.GetDetailsAsync(normalizedBillNo);
            var paymentDetails = new List<PaymentDetail>();
            if (details.Any())
            {
                var detailAmounts = details
                    .Select(d => d.subTotal ?? (decimal)(d.Price * d.Qty))
                    .ToList();
                var allocatedAmounts = AllocatePayment(payAmount, detailAmounts);

                for (var i = 0; i < details.Count(); i++)
                {
                    var detail = details.ElementAt(i);
                    var lineAmount = allocatedAmounts[i];
                    if (lineAmount <= 0)
                        continue;

                    var paymentDetail = new PaymentDetail
                    {
                        ReceiptNo = receiptNo,
                        billNO = normalizedBillNo,
                        ReceiptDate = now,
                        AmountPaid = lineAmount,
                        AccountNo = accountNo,
                        RevType = detail.Category ?? accountNo,
                        isPost = false,
                        AmountToPay = lineAmount,
                        BillItem = detail.drgName,
                        BillDate = billing?.bDate.ToDateTime(TimeOnly.MinValue) ?? now.Date,
                        SNoID = detail.SNO > 0 ? detail.SNO : null,
                        suppres = false
                    };
                    paymentDetails.Add(paymentDetail);
                    context.PaymentDetails.Add(paymentDetail);
                }
            }
            else
            {
                var paymentDetail = new PaymentDetail
                {
                    ReceiptNo = receiptNo,
                    billNO = normalizedBillNo,
                    ReceiptDate = now,
                    AmountPaid = payAmount,
                    AccountNo = accountNo,
                    RevType = accountNo,
                    isPost = false,
                    AmountToPay = payAmount,
                    BillItem = paymentFor,
                    BillDate = billing?.bDate.ToDateTime(TimeOnly.MinValue) ?? now.Date,
                    suppres = false
                };
                paymentDetails.Add(paymentDetail);
                context.PaymentDetails.Add(paymentDetail);
            }

            var tranId = Guid.NewGuid().ToString("N")[..12].ToUpper();
            var paymentType = new PaymentType
            {
                ReceiptNo = receiptNo,
                AmountPaid = payAmount,
                PayType = payType,
                ReceiptDate = now,
                isPost = false,
                AccountNo = accountNo,
                suppres = false,
                reversed = false,
                EntryDate = now,
                EntryTime = now,
                ClientName = patientName,
                AppName = "AestheticEMR",
                TranID = tranId
            };
            context.PaymentTypes.Add(paymentType);

            if (billing is not null)
            {
                var previouslyPaid = await context.Payments
                    .AsNoTracking()
                    .Where(p => p.billNO == normalizedBillNo && p.suppres != true && p.ReceiptNo != receiptNo)
                    .SumAsync(p => (decimal?)p.AmountPaid) ?? 0;

                var totalPaid = previouslyPaid + payAmount;
                billing.AmountPaid = totalPaid;
                var newBalance = debtBF + amountBilled + tax - discount - totalPaid;
                billing.isPaid = newBalance <= 0;
            }

            await context.SaveChangesAsync();

            _logger.LogInformation("Receipt {ReceiptNo} saved for bill {BillNo}", receiptNo, normalizedBillNo);

            var posted = await receiptAccountingPostingService.PostReceiptAsync(new ReceiptAccountingPostRequest
            {
                ReceiptNo = receiptNo,
                BillNo = normalizedBillNo,
                TranId = tranId,
                PayType = payType,
                Amount = payAmount,
                EntryDate = now,
                CoyId = billing?.clientID ?? vwhRecord?.RetainCode ?? defaults.Values.GetValueOrDefault("CoyID", "0001"),
                ReceivableAccountNo = vwhRecord?.AcctId,
                BankAccountNo = accountNo,
                PatientName = patientName
            });

            if (posted)
            {
                payment.isPost = true;
                paymentType.isPost = true;
                foreach (var pd in paymentDetails)
                {
                    pd.isPost = true;
                }
                await context.SaveChangesAsync();
                _logger.LogInformation("Receipt {ReceiptNo} marked posted to accounting", receiptNo);
            }

            return CreatedAtAction(nameof(GetByBillNo), new { billNo = normalizedBillNo }, new ReceiptSavedVM
            {
                ReceiptNo = receiptNo,
                ReceiptDate = now,
                AmountPaid = payAmount,
                PayType = payType
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving receipt for {BillNo}", billNo);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpGet("receipts")]
    [ProducesResponseType(typeof(IEnumerable<QryhBillingIncomeVM>), 200)]
    public async Task<IActionResult> GetReceipts([FromQuery] bool includeAll = false, [FromQuery] string? retainId = null)
    {
        try
        {
            var query = context.QryhBillingIncomes
                .AsNoTracking()
                .Where(x => x.Suppres != true);

            if (!string.IsNullOrWhiteSpace(retainId))
            {
                var retainership = await retainershipService.GetByIdAsync(retainId);
                if (retainership is null)
                    return NotFound(retainId);

                var companyKeys = new[]
                {
                    retainership.RetainId,
                    retainership.RetainCode,
                    retainership.RetainName,
                    retainership.ClientName
                }
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

                if (companyKeys.Count > 0)
                {
                    query = query.Where(x => x.Coyname != null && companyKeys.Contains(x.Coyname));
                }
            }

            if (!includeAll)
            {
                var today = DateTime.Today;
                query = query.Where(x => x.ReceiptDate.Date == today);
            }

            var rows = await query
                .OrderByDescending(x => x.ReceiptDate)
                .ThenByDescending(x => x.RTime)
                .ToListAsync();

            var receiptNos = rows.Select(x => x.ReceiptNo).Distinct().ToList();
            var paymentMap = receiptNos.Count == 0
                ? new Dictionary<string, Payment>(StringComparer.OrdinalIgnoreCase)
                : await context.Payments
                    .AsNoTracking()
                    .Where(x => receiptNos.Contains(x.ReceiptNo) && x.suppres != true)
                    .ToDictionaryAsync(x => x.ReceiptNo, StringComparer.OrdinalIgnoreCase);

            var paymentTypeMap = receiptNos.Count == 0
                ? new Dictionary<string, PaymentType>(StringComparer.OrdinalIgnoreCase)
                : await context.PaymentTypes
                    .AsNoTracking()
                    .Where(x => receiptNos.Contains(x.ReceiptNo) && x.suppres != true)
                    .ToDictionaryAsync(x => x.ReceiptNo, StringComparer.OrdinalIgnoreCase);

            // Collect all distinct billNos for a single batch reference check
            var billNos = rows.Select(x => x.BillNo).Distinct().ToList();

            var referencedBillNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var billingSnapshotMap = new Dictionary<string, (decimal AmountBilled, decimal AmountPaid, decimal DebtBf, decimal Discount, decimal Tax)>(StringComparer.OrdinalIgnoreCase);

            if (billNos.Count > 0)
            {
                // A billNo is "in use" if it is referenced in any of:
                // HRecords (attendance), Billings, HDental, HConsulting
                // (Payments table itself is OK to have — that's the receipt data)
                var inHRecords    = await context.HRecords.AsNoTracking()
                    .Where(x => billNos.Contains(x.ConsultId) && x.Suppres != true)
                    .Select(x => x.ConsultId)
                    .ToListAsync();

                var inBillings    = await context.Billings.AsNoTracking()
                    .Where(x => billNos.Contains(x.billNO))
                    .Select(x => x.billNO)
                    .ToListAsync();

                var inDental      = await context.HDentals.AsNoTracking()
                    .Where(x => billNos.Contains(x.ConsultId))
                    .Select(x => x.ConsultId)
                    .ToListAsync();

                var inConsulting  = await context.HConsultings.AsNoTracking()
                    .Where(x => billNos.Contains(x.ConsultId))
                    .Select(x => x.ConsultId)
                    .ToListAsync();

                foreach (var id in inHRecords.Concat(inBillings).Concat(inDental).Concat(inConsulting))
                    referencedBillNos.Add(id);

                // Fetch live billing snapshot values (billed/paid/balance) from Billings table
                var billingSnapshots = await context.Billings.AsNoTracking()
                    .Where(x => billNos.Contains(x.billNO))
                    .Select(x => new { BillNo = x.billNO, x.AmountBilled, x.AmountPaid, x.DebtBF, x.Discount, Tax = x.Tax ?? 0 })
                    .ToListAsync();

                foreach (var bs in billingSnapshots)
                {
                    if (!billingSnapshotMap.ContainsKey(bs.BillNo))
                        billingSnapshotMap[bs.BillNo] = (bs.AmountBilled ?? 0, bs.AmountPaid ?? 0, bs.DebtBF ?? 0, bs.Discount ?? 0, (decimal)bs.Tax);
                }
            }

            var vms = rows.Select(x =>
            {
                var hasLiveBilling = billingSnapshotMap.TryGetValue(x.BillNo, out var billing);
                var payment = paymentMap.GetValueOrDefault(x.ReceiptNo);
                var paymentType = paymentTypeMap.GetValueOrDefault(x.ReceiptNo);
                var amountBilled = hasLiveBilling ? billing.AmountBilled : x.AmountBilled;
                var amountPaid = x.AmountPaid;
                var tax = hasLiveBilling ? billing.Tax : 0;
                var balance = hasLiveBilling
                    ? billing.DebtBf + amountBilled + tax - billing.Discount - (billing.AmountPaid)
                    : (x.Balance ?? 0);

                return new QryhBillingIncomeVM
                {
                    ReceiptDate = x.ReceiptDate,
                    RTime = x.RTime,
                    ReceiptNo = x.ReceiptNo,
                    PNo = x.Pno,
                    PaymentFor = x.PaymentFor,
                    AmountBilled = amountBilled,
                    Tax = tax,
                    AmountPaid = amountPaid,
                    Balance = balance,
                    PayType = x.PayType,
                    ClinicId = x.ClinicId,
                    Fullname = x.Fullname,
                    PatNo = x.PatNo,
                    ReceivedBy = x.Receivedby,
                    BillNo = x.BillNo,
                    CoyName = x.Coyname,
                    IsPost = x.IsPost,
                    Remarks = payment?.Remarks ?? x.Remarks,
                    AccountNo = paymentType?.AccountNo ?? x.AcctId,
                    ChequeNo = payment?.ChequeNo,
                    BankCode = payment?.BankCode,
                    ValueDate = payment?.ValueDate,
                    Suppres = x.Suppres,
                    CanDelete = !referencedBillNos.Contains(x.BillNo)
                };
            }).ToList();

            return Ok(vms);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving receipts");
            AddModelError("Unable to retrieve receipts");
            return BadRequest(ModelState);
        }
    }

    /// <summary>Update mutable fields of an existing receipt.</summary>
    [HttpPut("receipts/{receiptNo}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateReceipt(string receiptNo, [FromBody] UpdateReceiptVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var payTypeValue = (model.PayType ?? string.Empty).Trim();
            var isCashPayment = string.Equals(payTypeValue, "Cash", StringComparison.OrdinalIgnoreCase);
            if (!isCashPayment && string.IsNullOrWhiteSpace(model.AccountNo))
            {
                AddModelError("Bank Account is required for non-cash payment.");
                return BadRequest(ModelState);
            }

            var payment = await context.Payments.FirstOrDefaultAsync(p => p.ReceiptNo == receiptNo);
            if (payment is null)
                return NotFound(receiptNo);

            payment.payType = payTypeValue;
            payment.Remarks = model.Remarks;
            payment.Receivedby = model.ReceivedBy;
            payment.ChequeNo = model.ChequeNo;
            payment.BankCode = model.BankCode;
            payment.ValueDate = model.ValueDate;
            payment.ChequeDate = model.ValueDate;

            var payType = await context.PaymentTypes.FirstOrDefaultAsync(pt => pt.ReceiptNo == receiptNo);
            if (payType is not null)
            {
                payType.PayType = payTypeValue;
                payType.AccountNo = model.AccountNo?.Trim();
            }

            var paymentDetails = await context.PaymentDetails
                .Where(pd => pd.ReceiptNo == receiptNo)
                .ToListAsync();
            foreach (var paymentDetail in paymentDetails)
            {
                paymentDetail.AccountNo = model.AccountNo?.Trim() ?? paymentDetail.AccountNo;
            }

            await context.SaveChangesAsync();

            _logger.LogInformation("Receipt {ReceiptNo} updated", receiptNo);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating receipt {ReceiptNo}", receiptNo);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    private static List<decimal> AllocatePayment(decimal totalAmount, IList<decimal> lineAmounts)
    {
        var allocations = Enumerable.Repeat(0m, lineAmounts.Count).ToList();
        if (totalAmount <= 0 || lineAmounts.Count == 0)
        {
            return allocations;
        }

        var totalLineAmount = lineAmounts.Sum();
        if (totalLineAmount <= 0)
        {
            allocations[0] = totalAmount;
            return allocations;
        }

        decimal allocatedSoFar = 0;
        for (var i = 0; i < lineAmounts.Count; i++)
        {
            var lineAmount = Math.Max(0, lineAmounts[i]);
            decimal allocated;
            if (i == lineAmounts.Count - 1)
            {
                allocated = totalAmount - allocatedSoFar;
            }
            else
            {
                allocated = Math.Round(totalAmount * (lineAmount / totalLineAmount), 2, MidpointRounding.AwayFromZero);
                allocated = Math.Min(allocated, totalAmount - allocatedSoFar);
            }

            allocations[i] = allocated;
            allocatedSoFar += allocated;
        }

        return allocations;
    }

    private static string AmountToWords(decimal amount)
    {
        return amount.ToString("N2", CultureInfo.InvariantCulture);
    }

    [HttpGet("vwh-record/{consultId}")]
    [ProducesResponseType(typeof(VwhRecordSummaryVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetVwhRecordSummary(string consultId)
    {
        try
        {
            var normalizedConsultId = consultId?.Trim();
            if (string.IsNullOrWhiteSpace(normalizedConsultId))
            {
                return NotFound(consultId);
            }

            var record = await context.VwhRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ConsultId == normalizedConsultId);

            if (record is null)
                return NotFound(consultId);

            string? patientPhoto = null;
            if (!string.IsNullOrWhiteSpace(record.PNo))
            {
                var patient = await context.HPatients
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Pno == record.PNo);

                if (patient?.PatPix != null && patient.PatPix.Length > 0)
                {
                    patientPhoto = $"data:image/jpeg;base64,{Convert.ToBase64String(patient.PatPix)}";
                }
            }

            return Ok(new VwhRecordSummaryVM
            {
                ConsultId = record.ConsultId,
                PNo = record.PNo,
                ClientCat = record.ClientCat,
                ClinicType = record.ClinicType,
                Coyname = record.Coyname,
                RetainName = record.RetainName,
                Fullname = record.Fullname,
                Dob = record.Dob,
                Age = record.Age,
                PhoneNo = record.PhoneNo,
                RetainCode = record.RetainCode,
                RetainId = record.RetainId,
                PatientPhotoBase64 = patientPhoto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving VwhRecord summary for {ConsultId}", consultId);
            AddModelError("Unable to retrieve attendance summary");
            return BadRequest(ModelState);
        }
    }
}
