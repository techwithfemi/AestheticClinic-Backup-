using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDental
{
    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DOB { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    public int SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dDate { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(100)]
    public string Reason { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    public bool? isDischarged { get; set; }
}
