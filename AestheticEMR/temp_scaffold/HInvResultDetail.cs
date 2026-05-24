using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInvResultDetail
{
    public long Id { get; set; }

    public string Labno { get; set; } = null!;

    public string? Description { get; set; }

    public string? Result { get; set; }

    public string? Desc2 { get; set; }

    public string? Sample { get; set; }

    public string? Class { get; set; }

    public string? Range { get; set; }

    public string? Remarks { get; set; }

    public long? SnoId { get; set; }

    public int? SerialNo { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? SubClass { get; set; }

    public long? SubClassId { get; set; }
}
