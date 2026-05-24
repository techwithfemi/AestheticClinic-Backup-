using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwEmployee
{
    public string EmpNo { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public string? Authorizer { get; set; }

    public string DeptName { get; set; } = null!;

    public string EmpCat { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public string HomeAddress { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string StatName { get; set; } = null!;
}
