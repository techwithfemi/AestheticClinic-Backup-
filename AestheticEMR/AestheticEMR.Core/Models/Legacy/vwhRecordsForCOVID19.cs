using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhRecordsForCOVID19
{
    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(1100)]
    [Unicode(false)]
    public string? HomeAddress { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Maturity { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string? ClientType { get; set; }
}
