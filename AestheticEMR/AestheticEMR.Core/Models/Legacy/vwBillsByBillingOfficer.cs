using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillsByBillingOfficer
{
    [Column(TypeName = "datetime")]
    public DateTime BillDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RecDate { get; set; }

    [StringLength(100)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(251)]
    public string Fullname { get; set; } = null!;

    [StringLength(550)]
    public string drgName { get; set; } = null!;

    public double subTotal { get; set; }

    [StringLength(101)]
    public string? EmpName { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;
}
