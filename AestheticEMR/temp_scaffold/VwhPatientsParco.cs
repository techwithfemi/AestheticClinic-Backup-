using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhPatientsParco
{
    public string Pno { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PostCode { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string OtherNames { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public string Client { get; set; } = null!;

    public string BillingCat { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string PatCat { get; set; } = null!;

    public string Title { get; set; } = null!;

    public byte[]? Picture { get; set; }
}
