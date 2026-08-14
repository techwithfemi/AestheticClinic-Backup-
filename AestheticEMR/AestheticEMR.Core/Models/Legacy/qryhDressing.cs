using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDressing
{
    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string PNO { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime drDate { get; set; }

    [StringLength(250)]
    public string drName { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string dressedby { get; set; } = null!;

    public bool attendedTo { get; set; }

    public int ID { get; set; }
}
