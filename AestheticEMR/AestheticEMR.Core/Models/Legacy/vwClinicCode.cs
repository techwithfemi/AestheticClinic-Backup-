using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwClinicCode
{
    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string ClinicType { get; set; } = null!;

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(2)]
    public string? RctCode { get; set; }

    [StringLength(50)]
    public string ClinicID { get; set; } = null!;
}
