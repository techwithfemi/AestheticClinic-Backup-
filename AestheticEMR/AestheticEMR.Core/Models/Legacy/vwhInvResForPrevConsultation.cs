using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvResForPrevConsultation
{
    public long ID { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    public string? investigate { get; set; }

    public string? invResultX { get; set; }

    [StringLength(4000)]
    public string invResult { get; set; } = null!;

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(50)]
    public string treatedBy { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string? RANGE { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? Unit { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? DESCRIPTION { get; set; }

    public long invResID { get; set; }
}
