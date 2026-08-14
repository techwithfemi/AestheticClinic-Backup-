using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("OutboxState")]
[Index("BusName", "Created", Name = "IX_OutboxState_BusName_Created")]
[Index("Created", Name = "IX_OutboxState_Created")]
public partial class OutboxState
{
    [Key]
    public Guid OutboxId { get; set; }

    public Guid LockId { get; set; }

    public byte[]? RowVersion { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Delivered { get; set; }

    public long? LastSequenceNumber { get; set; }

    [StringLength(256)]
    public string? BusName { get; set; }

    [InverseProperty("Outbox")]
    public virtual ICollection<OutboxMessage> OutboxMessages { get; set; } = new List<OutboxMessage>();
}
