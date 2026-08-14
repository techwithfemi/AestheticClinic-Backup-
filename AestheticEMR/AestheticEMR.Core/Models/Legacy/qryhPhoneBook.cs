using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPhoneBook
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(1001)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(500)]
    public string? Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    public int? Age { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string Clinic { get; set; } = null!;
}
