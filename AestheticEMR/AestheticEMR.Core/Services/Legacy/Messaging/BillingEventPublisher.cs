using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Messaging.Events;
using MassTransit;

namespace AestheticEMR.Core.Services.Legacy.Messaging;

public class BillingEventPublisher(IPublishEndpoint publishEndpoint)
{
    public async Task PublishUpsertedAsync(
        Billing billing,
        IReadOnlyCollection<BillingDetail> details,
        CancellationToken cancellationToken = default)
    {
        var evt = new BillingUpsertedEvent
        {
            BillNo = billing.billNO,
            BDate = billing.bDate,
            PNo = billing.pNo,
            ClientId = billing.clientID,
            DebtBF = billing.DebtBF ?? 0,
            AmountBilled = billing.AmountBilled ?? 0,
            Discount = billing.Discount ?? 0,
            AmountPaid = billing.AmountPaid ?? 0,
            BillType = billing.billType,
            IsPaid = billing.isPaid ?? false,
            IsProcess = billing.isProcess ?? false,
            AdmDate = billing.AdmDate,
            DischDate = billing.DischDate,
            TimeVal = billing.timeVal,
            ApprvCode = billing.ApprvCode,
            IsPost = billing.isPost ?? false,
            Details = details.Select(d => new BillingDetailPayload
            {
                BillNo = d.billNO,
                SNO = d.SNO,
                TranID = d.TranID,
                DtDate = d.dtDate,
                DrgName = d.drgName,
                Price = d.Price,
                Qty = d.Qty,
                SubTotal = d.subTotal,
                BillType = d.billType,
                ConId = d.conID,
                RevType = d.revType,
                BillTo = d.BillTo,
                CoyName = d.CoyName,
                BillBy = d.BillBy
            }).ToList()
        };

        await publishEndpoint.Publish(evt, cancellationToken);
    }

    public async Task PublishDeletedAsync(
        string billNo,
        string pNo,
        IReadOnlyCollection<string> tranIds,
        CancellationToken cancellationToken = default)
    {
        await publishEndpoint.Publish(new BillingDeletedEvent
        {
            BillNo = billNo,
            PNo = pNo,
            TranIds = tranIds
        }, cancellationToken);
    }
}
