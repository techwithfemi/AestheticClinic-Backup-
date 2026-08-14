using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPrescriptionInfoListAnaest
{
    public long ID { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? CTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    public long? ConID { get; set; }
}
