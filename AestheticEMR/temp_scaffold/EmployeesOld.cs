using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmployeesOld
{
    public string EmpId { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public string DeptId { get; set; } = null!;

    public string? EmpStatus { get; set; }

    public DateTime? HireDate { get; set; }

    public DateTime? Dob { get; set; }

    public double? GrossSal { get; set; }

    public string? Nhsno { get; set; }

    public string? Nsitfno { get; set; }

    public string? JobDesc { get; set; }

    public string? FirstGrtname { get; set; }

    public string? FirstGrtAddress { get; set; }

    public string? SecondGrtName { get; set; }

    public string? SecondGrtAddress { get; set; }

    public string? EmpCat { get; set; }

    public double? MedAllw { get; set; }

    public string? Sex { get; set; }

    public string? SpouseName { get; set; }

    public string? MaidenName { get; set; }

    public DateTime? DtExpectedConfirm { get; set; }

    public string? HomeAddress { get; set; }

    public string? Marital { get; set; }

    public string? Idval { get; set; }

    public string? Authorizer { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
}
