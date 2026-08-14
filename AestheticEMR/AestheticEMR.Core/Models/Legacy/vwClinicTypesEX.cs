using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwClinicTypesEX
{
    [StringLength(100)]
    public string ClinicName { get; set; } = null!;

    [StringLength(50)]
    public string ClinicID { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string? IDValCode { get; set; }
}
