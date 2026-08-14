using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hVaccineList")]
public partial class hVaccineList
{
    public long SNo { get; set; }

    [StringLength(250)]
    public string Vaccine { get; set; } = null!;
}
