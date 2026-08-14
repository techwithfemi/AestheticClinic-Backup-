using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Ebony_hPatient
{
    [StringLength(100)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? OldPNo { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string pSurName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(1100)]
    [Unicode(false)]
    public string? HomeAddress { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string MiddleName { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string StreetAddress { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string State { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string StateOfOrigin { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Occupation { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? OfficeAddress { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? NextofKin { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? kinAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? relationToKin { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? BloodGroup { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Genotype { get; set; }

    [StringLength(500)]
    public string? email { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? NOKPhone { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(406)]
    public string FullName { get; set; } = null!;
}
