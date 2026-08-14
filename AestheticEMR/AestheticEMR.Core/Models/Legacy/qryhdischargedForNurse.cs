using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhdischargedForNurse
{
    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string WardID { get; set; } = null!;

    [StringLength(4250)]
    [Unicode(false)]
    public string SummDischbyNurse { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? ApprvBy { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(101)]
    public string? ApprovedBy { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    public long ID { get; set; }
}
