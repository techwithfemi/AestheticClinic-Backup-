using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhPatientsParco
{
    [StringLength(20)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

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

    [StringLength(80)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string OtherNames { get; set; } = null!;

    [StringLength(101)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string PhoneNo { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Client { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string BillingCat { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string PatCat { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "image")]
    public byte[]? Picture { get; set; }
}
