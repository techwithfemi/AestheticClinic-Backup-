using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingDetailsForBill
{
    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    public double Qty { get; set; }

    public double Price { get; set; }

    public double totPrice { get; set; }
}
