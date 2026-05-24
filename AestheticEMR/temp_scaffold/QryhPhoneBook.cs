using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPhoneBook
{
    public DateTime Date { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public int? Age { get; set; }

    public string Company { get; set; } = null!;

    public string Clinic { get; set; } = null!;
}
