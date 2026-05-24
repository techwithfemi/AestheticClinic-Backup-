using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhComplaintsForCovid
{
    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Phone { get; set; }

    public string? HomeAddress { get; set; }

    public string? Sex { get; set; }

    public string? Symptoms { get; set; }

    public string? Complaints { get; set; }

    public string? Diagnosis { get; set; }
}
