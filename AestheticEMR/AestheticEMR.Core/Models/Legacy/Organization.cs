using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("Organization")]
public partial class Organization
{
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string OrgID { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string OrgName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? OrgAddress1 { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? OrgAddress2 { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? OrgAddress3 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? OrgLogo { get; set; }

    [Column(TypeName = "image")]
    public byte[]? imgLogo { get; set; }
}
