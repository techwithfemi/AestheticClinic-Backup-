using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryEmployee
{
    public int? Hireage { get; set; }

    public string EmpId { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public string DeptId { get; set; } = null!;

    public string EmpStatus { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public DateTime Dob { get; set; }

    public double GrossSal { get; set; }

    public string? Nhsno { get; set; }

    public string? Nsitfno { get; set; }

    public string JobDesc { get; set; } = null!;

    public string FirstGrtname { get; set; } = null!;

    public string FirstGrtAddress { get; set; } = null!;

    public string SecondGrtName { get; set; } = null!;

    public string SecondGrtAddress { get; set; } = null!;

    public string EmpCat { get; set; } = null!;

    public double MedAllw { get; set; }

    public string Sex { get; set; } = null!;

    public string? SpouseName { get; set; }

    public string? MaidenName { get; set; }

    public DateTime? DtExpectedConfirm { get; set; }

    public string HomeAddress { get; set; } = null!;

    public string Marital { get; set; } = null!;

    public int? Dobage { get; set; }
}
