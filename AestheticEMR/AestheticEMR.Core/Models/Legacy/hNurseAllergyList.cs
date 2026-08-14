using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("hNurseAllergyList")]
public partial class hNurseAllergyList
{
    [Key]
    [StringLength(500)]
    [Unicode(false)]
    public string Allergy { get; set; } = null!;
}
