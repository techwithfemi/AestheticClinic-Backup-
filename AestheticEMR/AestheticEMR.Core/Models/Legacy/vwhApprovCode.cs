using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhApprovCode
{
    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprvDate { get; set; }

    [StringLength(50)]
    public string? ConsultID { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ApprvCode { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(50)]
    public string? BillType { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    public long SNO { get; set; }
}
