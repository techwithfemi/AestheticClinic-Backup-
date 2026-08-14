using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhAdmissionHist
{
    [Column(TypeName = "datetime")]
    public DateTime AdmDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? aTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;
}
