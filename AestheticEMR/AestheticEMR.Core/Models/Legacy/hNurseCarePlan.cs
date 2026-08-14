using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hNurseCarePlan")]
public partial class hNurseCarePlan
{
    public int ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtTime { get; set; }

    [StringLength(50)]
    public string? pno { get; set; }

    [StringLength(50)]
    public string? consultID { get; set; }

    [StringLength(250)]
    public string? NurDiag { get; set; }

    [StringLength(250)]
    public string? Objective { get; set; }

    [StringLength(250)]
    public string? nurOrders { get; set; }

    [StringLength(250)]
    public string? NurEval { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }
}
