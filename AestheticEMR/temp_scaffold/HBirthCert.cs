using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HBirthCert
{
    public long Sno { get; set; }

    public string? Pno { get; set; }

    public DateTime? Dob { get; set; }

    public DateTime? Tob { get; set; }

    public string? Wt { get; set; }

    public string? MothersName { get; set; }

    public string? FathersName { get; set; }

    public DateTime? EntryDate { get; set; }
}
