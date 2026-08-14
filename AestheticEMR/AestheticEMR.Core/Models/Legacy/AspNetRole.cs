using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class AspNetRole
{
    [Key]
    public string Id { get; set; } = null!;

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    [StringLength(256)]
    public string? Name { get; set; }

    [StringLength(256)]
    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
