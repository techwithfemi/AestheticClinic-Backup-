using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class DrugCategory
{
    [Key]
    [StringLength(255)]
    [Unicode(false)]
    public string drgCatName { get; set; } = null!;

    [StringLength(150)]
    public string? catRemarks { get; set; }

    [StringLength(50)]
    public string? drgCatGroup { get; set; }

    [StringLength(50)]
    public string? deptBillCenter { get; set; }

    [StringLength(2)]
    public string? drgCatCode { get; set; }

    [StringLength(500)]
    public string? RptHead { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DeptID { get; set; }
}
