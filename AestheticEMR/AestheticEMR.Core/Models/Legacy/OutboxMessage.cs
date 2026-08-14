using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("OutboxMessage")]
[Index("EnqueueTime", Name = "IX_OutboxMessage_EnqueueTime")]
[Index("ExpirationTime", Name = "IX_OutboxMessage_ExpirationTime")]
public partial class OutboxMessage
{
    [Key]
    public long SequenceNumber { get; set; }

    public DateTime? EnqueueTime { get; set; }

    public DateTime SentTime { get; set; }

    public string? Headers { get; set; }

    public string? Properties { get; set; }

    public Guid? InboxMessageId { get; set; }

    public Guid? InboxConsumerId { get; set; }

    public Guid? OutboxId { get; set; }

    public Guid MessageId { get; set; }

    [StringLength(256)]
    public string ContentType { get; set; } = null!;

    public string MessageType { get; set; } = null!;

    public string Body { get; set; } = null!;

    public Guid? ConversationId { get; set; }

    public Guid? CorrelationId { get; set; }

    public Guid? InitiatorId { get; set; }

    public Guid? RequestId { get; set; }

    [StringLength(256)]
    public string? SourceAddress { get; set; }

    [StringLength(256)]
    public string? DestinationAddress { get; set; }

    [StringLength(256)]
    public string? ResponseAddress { get; set; }

    [StringLength(256)]
    public string? FaultAddress { get; set; }

    public DateTime? ExpirationTime { get; set; }

    [ForeignKey("InboxMessageId, InboxConsumerId")]
    [InverseProperty("OutboxMessages")]
    public virtual InboxState? InboxState { get; set; }

    [ForeignKey("OutboxId")]
    [InverseProperty("OutboxMessages")]
    public virtual OutboxState? Outbox { get; set; }
}
