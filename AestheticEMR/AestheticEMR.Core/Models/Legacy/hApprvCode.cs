using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hApprvCode")]
public partial class hApprvCode
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprvDate { get; set; }

    [StringLength(50)]
    public string? ConsultID { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ApprvCode { get; set; }

    [StringLength(50)]
    public string? PNo { get; set; }

    [StringLength(50)]
    public string? BillType { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
