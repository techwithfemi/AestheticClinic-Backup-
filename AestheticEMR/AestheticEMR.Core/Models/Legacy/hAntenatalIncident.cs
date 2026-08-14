using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hAntenatalIncident
{
    public long SNO { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string IncidentType { get; set; } = null!;

    public short ValueType { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string REmarks { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime IncDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string entryBy { get; set; } = null!;
}
