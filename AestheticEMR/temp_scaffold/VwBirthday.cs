using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBirthday
{
    public string Code { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime? Dob { get; set; }

    public string? Email { get; set; }

    public int? Age { get; set; }

    public string? Company { get; set; }
}
