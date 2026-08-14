using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDischargeForNurseDailySumm
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ClientCat { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string WardID { get; set; } = null!;

    [StringLength(4250)]
    [Unicode(false)]
    public string SummDischbyNurse { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    [StringLength(101)]
    public string? StaffName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? StaffNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string WhatDAy { get; set; } = null!;
}
