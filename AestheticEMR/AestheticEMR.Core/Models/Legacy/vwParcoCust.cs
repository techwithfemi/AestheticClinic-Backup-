using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwParcoCust
{
    [StringLength(20)]
    [Unicode(false)]
    public string No_ { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [Column("Post Code")]
    [StringLength(20)]
    [Unicode(false)]
    public string Post_Code { get; set; } = null!;
}
