using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("InboxState")]
[Index("MessageId", "ConsumerId", Name = "AK_InboxState_MessageId_ConsumerId", IsUnique = true)]
[Index("Delivered", Name = "IX_InboxState_Delivered")]
public partial class InboxState
{
    [Key]
    public long Id { get; set; }

    public Guid MessageId { get; set; }

    public Guid ConsumerId { get; set; }

    public Guid LockId { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime Received { get; set; }

    public int ReceiveCount { get; set; }

    public DateTime? ExpirationTime { get; set; }

    public DateTime? Consumed { get; set; }

    public DateTime? Delivered { get; set; }

    public long? LastSequenceNumber { get; set; }

    [InverseProperty("InboxState")]
    public virtual ICollection<OutboxMessage> OutboxMessages { get; set; } = new List<OutboxMessage>();
}
