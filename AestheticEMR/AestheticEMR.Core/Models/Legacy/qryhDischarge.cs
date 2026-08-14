using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDischarge
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dischTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? WardID { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(3999)]
    [Unicode(false)]
    public string? ChiefCompaints { get; set; }

    [StringLength(3999)]
    [Unicode(false)]
    public string? DiagnosisFindings { get; set; }

    [StringLength(3999)]
    [Unicode(false)]
    public string? DrugsGiven { get; set; }

    [StringLength(3999)]
    [Unicode(false)]
    public string? ResponseToDrug { get; set; }

    [StringLength(3999)]
    [Unicode(false)]
    public string? Recommend { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? StaffNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(101)]
    public string StaffName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Reason { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ClientCat { get; set; } = null!;
}
