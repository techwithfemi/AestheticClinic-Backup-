using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhNurseRpt
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtTime { get; set; }

    [StringLength(50)]
    public string? Shift { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(500)]
    public string? RptHead { get; set; }

    [StringLength(500)]
    public string? SubHead { get; set; }

    [Column(TypeName = "text")]
    public string? Details { get; set; }

    [StringLength(3)]
    public string? Completed { get; set; }

    [StringLength(101)]
    public string? empFullname { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }

    public bool? isOLD { get; set; }
}
