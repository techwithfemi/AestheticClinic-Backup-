using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwResultForObsteAndPelvicScanPublic
{
    public long ID { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string HospName { get; set; } = null!;

    [StringLength(4000)]
    public string ResultMaster { get; set; } = null!;

    [StringLength(1000)]
    public string? Remarks { get; set; }

    [StringLength(116)]
    public string? EmpName { get; set; }

    [StringLength(250)]
    public string Description { get; set; } = null!;

    [StringLength(4000)]
    public string Result { get; set; } = null!;

    [StringLength(50)]
    public string? desc2 { get; set; }

    [StringLength(50)]
    public string? Class { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(61)]
    public string Fullname { get; set; } = null!;
}
