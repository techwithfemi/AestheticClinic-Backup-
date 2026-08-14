using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhFullnamePublic
{
    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(61)]
    public string Fullname { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string oldpNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string policyType { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string CoyType { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string pCatID { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string ClientCatID { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string homeAddress { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string officeAddress { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string DOB { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string occupation { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Ref { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string status { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string bloodGroup { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string genotype { get; set; } = null!;

    [StringLength(50)]
    public string empNo { get; set; } = null!;

    [StringLength(10)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstName { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    public double? Debt { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string branch { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string email { get; set; } = null!;

    [StringLength(50)]
    public string Maturity { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string drgRxn { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Title { get; set; } = null!;
}
