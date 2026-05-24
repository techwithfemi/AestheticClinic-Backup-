using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRecordsForCovid19
{
    public DateTime RecDate { get; set; }

    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Phone { get; set; }

    public string? HomeAddress { get; set; }

    public string? Sex { get; set; }

    public DateTime? Dob { get; set; }

    public string? Maturity { get; set; }

    public string? ClientCatId { get; set; }

    public string? ClientType { get; set; }
}
