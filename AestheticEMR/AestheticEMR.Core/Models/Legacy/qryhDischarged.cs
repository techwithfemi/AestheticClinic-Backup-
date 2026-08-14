using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDischarged
{
    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dischTime { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? WardID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Reason { get; set; }

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
    public string ClientCat { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Company { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Recommend { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? ResponseToDrug { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? DrugsGiven { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? DiagnosisFindings { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? ChiefCompaints { get; set; }

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;
}
