using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingDetail
{
    [StringLength(50)]
    public string? ConsultID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(50)]
    public string? pNO { get; set; }

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    [StringLength(50)]
    public string drgName { get; set; } = null!;

    [StringLength(50)]
    public string drgCatName { get; set; } = null!;

    public double Qty { get; set; }

    [StringLength(250)]
    public string usage { get; set; } = null!;

    public bool? attendedTo { get; set; }
}
