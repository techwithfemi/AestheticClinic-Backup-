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
    public async Task<IActionResult> GetPrintData(string billNo)
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

            if (balance == 0)
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
                payerPhone = patient.PPhoneNo ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(payerAddress))
            {
                payerAddress = $"{defaults.BillHead2}, {defaults.BillHead3}".Trim(',', ' ');
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
    /// Only PRIVATE patients are eligible; balance must be non-zero.
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
            // ── Load billing record ──────────────────────────────────────────
            var billing = await context.Billings.FirstOrDefaultAsync(x => x.billNO == billNo);
            if (billing is null)
                return NotFound(billNo);

            // ── Balance guard ────────────────────────────────────────────────
            var amountBilled = billing.AmountBilled ?? 0;
            var debtBF       = billing.DebtBF       ?? 0;
            var discount     = billing.Discount     ?? 0;
            var tax          = (decimal)(billing.Tax ?? 0);
            var amountPaid   = billing.AmountPaid   ?? 0;
            var balance      = debtBF + amountBilled + tax - discount - amountPaid;

            if (balance == 0)
            {
                AddModelError("Receipt cannot be issued: balance is zero.");
                return UnprocessableEntity(ModelState);
            }

            // ── PRIVATE guard (clientCat from VwhRecord) ─────────────────────
            var vwhRecord = await context.VwhRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.ConsultId == billNo);

            var defaults  = await emrAppDefaultsService.GetAsync();
            var clientCat = (vwhRecord?.ClientCat ?? string.Empty).Trim();

            if (!string.Equals(clientCat, defaults.ClientCategoryPrivate, StringComparison.OrdinalIgnoreCase))
            {
                AddModelError($"Receipts can only be issued to PRIVATE patients. This patient's category is '{clientCat}'.");
                return UnprocessableEntity(ModelState);
            }

            // ── Build receipt number ─────────────────────────────────────────
            // Format: RCT-{billNo}-{yyyyMMddHHmmss}
            var now       = DateTime.Now;
            var receiptNo = $"RCT-{billNo}-{now:yyyyMMddHHmmss}";

            // Collision guard (unlikely but safe)
            var collision = await context.Payments.AnyAsync(p => p.ReceiptNo == receiptNo);
            if (collision)
                receiptNo = $"RCT-{billNo}-{now:yyyyMMddHHmmssfff}";

            // ── Amount being paid: use provided amount or fall back to full balance ──
            var payAmount = (model.AmountToPay.HasValue && model.AmountToPay.Value > 0)
                ? model.AmountToPay.Value
                : balance;

            // ── Patient info for denormalised columns ────────────────────────
            var patient     = await patientService.GetByIdAsync(billing.pNo);
            var patientName = patient is null
                ? string.Empty
                : $"{patient.PSurName} {patient.PFirstname}".Trim();

            var receivedBy = string.IsNullOrWhiteSpace(model.ReceivedBy)
                ? GetCurrentUserId()
                : model.ReceivedBy;

            // ── Revenue account: use provided or fall back to first active ───
            var accountNo = model.AccountNo;
            if (string.IsNullOrWhiteSpace(accountNo))
            {
                var revType = await context.hRevenueTypes.AsNoTracking().FirstOrDefaultAsync();
                accountNo   = revType?.RevType ?? "CASH";
            }

            // ── 1. Payments table ────────────────────────────────────────────
            var payment = new Payment
            {
                ReceiptNo     = receiptNo,
                ReceiptDate   = now,
                billNO        = billNo,
                pNO           = billing.pNo,
                clinicID      = null,
                paymentFor    = $"Bill {billNo}",
                AmountBilled  = amountBilled,
                AmountPaid    = payAmount,
                AmountInWord  = AmountToWords(payAmount),
                Receivedby    = receivedBy,
                payType       = model.PayType,
                rTime         = now,
                Remarks       = model.Remarks,
                RetainCode    = billing.clientID,
                ChequeNo      = model.ChequeNo,
                ValueDate     = model.ValueDate,
                BankCode      = model.BankCode,
                ChequeDate    = model.ValueDate,
                isPost        = false,
                EntryDate     = now,
                EntryTime     = now,
                ClientName    = patientName,
                AppName       = "AestheticEMR",
                suppres       = false
            };
            context.Payments.Add(payment);

            // ── 2. PaymentDetails table (one row per billing detail line) ────
            var details = await billingService.GetDetailsAsync(billNo);
            long sno    = 1;
            foreach (var detail in details)
            {
                context.PaymentDetails.Add(new PaymentDetail
                {
                    SNo         = sno++,
                    ReceiptNo   = receiptNo,
                    billNO      = billNo,
                    ReceiptDate = now,
                    AmountPaid  = detail.subTotal ?? (decimal)(detail.Price * detail.Qty),
                    AccountNo   = accountNo,
                    RevType     = detail.Category ?? accountNo,
                    isPost      = false,
                    AmountToPay = detail.subTotal ?? (decimal)(detail.Price * detail.Qty),
                    BillItem    = detail.drgName,
                    BillDate    = billing.bDate.ToDateTime(TimeOnly.MinValue),
                    SNoID       = detail.SNO > 0 ? detail.SNO : null,
                    suppres     = false
                });
            }

            // ── 3. PaymentTypes table (payment method record) ────────────────
            context.PaymentTypes.Add(new PaymentType
            {
                ReceiptNo   = receiptNo,
                AmountPaid  = payAmount,
                PayType     = model.PayType,
                ReceiptDate = now,
                isPost      = false,
                AccountNo   = accountNo,
                suppres     = false,
                reversed    = false,
                EntryDate   = now,
                EntryTime   = now,
                ClientName  = patientName,
                AppName     = "AestheticEMR",
                TranID      = model.ChequeNo ?? Guid.NewGuid().ToString("N")[..12].ToUpper()
            });

            // ── 4. Update billing.AmountPaid = sum of all receipts for this billNo ──
            // Sum existing persisted payments plus the one we just staged above.
            var previouslyPaid = await context.Payments
                .AsNoTracking()
                .Where(p => p.billNO == billNo && p.suppres != true && p.ReceiptNo != receiptNo)
                .SumAsync(p => (decimal?)p.AmountPaid) ?? 0;

            var totalPaid = previouslyPaid + payAmount;

            billing.AmountPaid = totalPaid;

            // Mark as fully paid when balance clears
            var newBalance = debtBF + amountBilled + tax - discount - totalPaid;
            billing.isPaid = newBalance <= 0;

            await context.SaveChangesAsync();

            _logger.LogInformation("Receipt {ReceiptNo} saved for bill {BillNo}", receiptNo, billNo);

            return CreatedAtAction(nameof(GetByBillNo), new { billNo }, new ReceiptSavedVM
            {
                ReceiptNo   = receiptNo,
                ReceiptDate = now,
                AmountPaid  = totalPaid,
                PayType     = model.PayType
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving receipt for {BillNo}", billNo);
            AddModelError("Unable to save receipt");
            return BadRequest(ModelState);
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    /// <summary>
    /// List receipts sourced from QryhBillingIncome.
    /// By default returns current-date records unless includeAll=true.
    /// When retainId is provided, results are filtered to that company.
    /// </summary>
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

            // Collect all distinct billNos for a single batch reference check
            var billNos = rows.Select(x => x.BillNo).Distinct().ToList();

            var referencedBillNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

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
            }

            var vms = rows.Select(x => new QryhBillingIncomeVM
            {
                ReceiptDate = x.ReceiptDate,
                RTime       = x.RTime,
                ReceiptNo   = x.ReceiptNo,
                PNo         = x.Pno,
                PaymentFor  = x.PaymentFor,
                AmountBilled= x.AmountBilled,
                AmountPaid  = x.AmountPaid,
                Balance     = x.Balance,
                PayType     = x.PayType,
                ClinicId    = x.ClinicId,
                Fullname    = x.Fullname,
                PatNo       = x.PatNo,
                ReceivedBy  = x.Receivedby,
                BillNo      = x.BillNo,
                CoyName     = x.Coyname,
                IsPost      = x.IsPost,
                Remarks     = x.Remarks,
                Suppres     = x.Suppres,
                CanDelete   = !referencedBillNos.Contains(x.BillNo)
            });

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
            var payment = await context.Payments.FirstOrDefaultAsync(p => p.ReceiptNo == receiptNo);
            if (payment is null)
                return NotFound(receiptNo);

            payment.payType    = model.PayType;
            payment.Remarks    = model.Remarks;
            payment.Receivedby = model.ReceivedBy;
            payment.ChequeNo   = model.ChequeNo;
            payment.BankCode   = model.BankCode;
            payment.ValueDate  = model.ValueDate;
            payment.ChequeDate = model.ValueDate;

            // Mirror update into PaymentTypes row for this receipt
            var payType = await context.PaymentTypes.FirstOrDefaultAsync(pt => pt.ReceiptNo == receiptNo);
            if (payType is not null)
                payType.PayType = model.PayType;

            await context.SaveChangesAsync();

            _logger.LogInformation("Receipt {ReceiptNo} updated", receiptNo);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating receipt {ReceiptNo}", receiptNo);
            AddModelError("Unable to update receipt");
            return BadRequest(ModelState);
        }
    }

    /// <summary>Delete (suppress) a receipt — blocked when billNo is still referenced in operational tables.</summary>
    [HttpDelete("receipts/{receiptNo}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> DeleteReceipt(string receiptNo)
    {
        try
        {
            var payment = await context.Payments.FirstOrDefaultAsync(p => p.ReceiptNo == receiptNo);
            if (payment is null)
                return NotFound(receiptNo);

            var billNo = payment.billNO;

            // Reference guard: block deletion when consultId is in use
            var isReferenced =
                await context.HRecords.AnyAsync(x => x.ConsultId == billNo && x.Suppres != true) ||
                await context.Billings.AnyAsync(x => x.billNO == billNo) ||
                await context.HDentals.AnyAsync(x => x.ConsultId == billNo) ||
                await context.HConsultings.AnyAsync(x => x.ConsultId == billNo);

            if (isReferenced)
            {
                AddModelError($"Receipt cannot be deleted: Bill No '{billNo}' is still referenced in operational records (attendance, billing, dental, or consulting).");
                return UnprocessableEntity(ModelState);
            }

            // Soft-delete
            payment.suppres = true;

            // Also suppress linked PaymentTypes rows
            var payTypes = await context.PaymentTypes
                .Where(pt => pt.ReceiptNo == receiptNo)
                .ToListAsync();
            foreach (var pt in payTypes)
                pt.suppres = true;

            await context.SaveChangesAsync();

            _logger.LogInformation("Receipt {ReceiptNo} suppressed", receiptNo);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting receipt {ReceiptNo}", receiptNo);
            AddModelError("Unable to delete receipt");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("vwh-record/{consultId}")]
    [ProducesResponseType(typeof(VwhRecordSummaryVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetVwhRecordSummary(string consultId)
    {
        try
        {
            var record = await context.VwhRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ConsultId == consultId);

            if (record is null)
                return NotFound(consultId);

            // Load patient photo from HPatient table
            string? patientPhoto = null;
            if (!string.IsNullOrEmpty(record.PNo))
            {
                var patient = await context.HPatients
                    .AsNoTracking()
                    .Where(p => p.Pno == record.PNo)
                    .FirstOrDefaultAsync();
                
                if (patient?.PatPix != null && patient.PatPix.Length > 0)
                {
                    // Convert byte array to base64 data URI
                    string base64String = Convert.ToBase64String(patient.PatPix);
                    patientPhoto = $"data:image/jpeg;base64,{base64String}";
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

    private static string AmountToWords(decimal amount)
    {
        // Simple implementation: "{amount:N2} only"
        // Replace with a proper currency-words library if needed.
        var absAmt = Math.Abs(amount);
        return $"{absAmt:N2} only";
    }

    /// <summary>
    /// Returns bank accounts from vwAccountsInfo for bank-account selection on receipts.
    /// Excludes the cash account (Acct_Cash) since that is selected implicitly for Cash payments.
    /// </summary>
    [HttpGet("bank-accounts")]
    [ProducesResponseType(typeof(IEnumerable<BankAccountVM>), 200)]
    public async Task<IActionResult> GetBankAccounts()
    {
        try
        {
            var defaults = await emrAppDefaultsService.GetAsync();
            var acctBanks = defaults.Get("Acct_Banks");

            var accountingConnStr = configuration.GetConnectionString("AccountingConnection")
                ?? throw new InvalidOperationException("AccountingConnection is not configured.");

            var accounts = new List<BankAccountVM>();
            await using var conn = new SqlConnection(accountingConnStr);
            await conn.OpenAsync();
            const string sql = "SELECT DISTINCT AccountName, AccountNo FROM vwAccountsInfo WHERE GroupId = @AcctBanks ORDER BY AccountName";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AcctBanks", acctBanks ?? string.Empty);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                accounts.Add(new BankAccountVM
                {
                    AccountName = reader.GetString(0),
                    AccountId   = reader.GetString(1)
                });
            }

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bank accounts");
            AddModelError("Unable to retrieve bank accounts");
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Returns the credit account ID (AcctId) from the PRIVATE company row in hRetainership.
    /// </summary>
    [HttpGet("private-credit-account")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPrivateCreditAccount()
    {
        try
        {
            var defaults = await emrAppDefaultsService.GetAsync();
            var privateCategory = defaults.ClientCategoryPrivate; // "PRIVATE"

            var retainership = await context.HRetainerships
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RetainName != null &&
                    r.RetainName.Trim().ToUpper() == privateCategory.ToUpper());

            if (retainership is null)
                return NotFound("PRIVATE retainership not found");

            return Ok(retainership.AcctId ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving private credit account");
            AddModelError("Unable to retrieve private credit account");
            return BadRequest(ModelState);
        }
    }
}
