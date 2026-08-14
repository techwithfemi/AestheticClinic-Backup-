using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDispDetailsForNurse1
{
    public long ID { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(50)]
    public string drgName { get; set; } = null!;

    public double Qty { get; set; }

    [StringLength(50)]
    public string? pNO { get; set; }

    [StringLength(250)]
    public string? usage { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string drgCatName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime mDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? mTime { get; set; }
}
