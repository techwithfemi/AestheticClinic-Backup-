using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hInvResultDetail
{
    public long ID { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string LABNO { get; set; } = null!;

    [StringLength(350)]
    [Unicode(false)]
    public string? DESCRIPTION { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? RESULT { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? DESC2 { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? SAMPLE { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? CLASS { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? RANGE { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? REMARKS { get; set; }

    public long? SNoID { get; set; }

    public int? SerialNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? SubClass { get; set; }

    public long? SubClassID { get; set; }
}
