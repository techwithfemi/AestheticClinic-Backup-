using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Employee
{
    public string EmpId { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? OtherName { get; set; }

    public string DeptId { get; set; } = null!;

    public string? UnitId { get; set; }

    public string Designation { get; set; } = null!;

    public string EmpCat { get; set; } = null!;

    public string EmpStatus { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public DateTime Dob { get; set; }

    public string? SalaryScale { get; set; }

    public double GrossSal { get; set; }

    public string? Nhsno { get; set; }

    public string? Nsitfno { get; set; }

    public string JobDesc { get; set; } = null!;

    public string FirstGrtname { get; set; } = null!;

    public string? FirstGrtAddress { get; set; }

    public string? SecondGrtName { get; set; }

    public string? SecondGrtAddress { get; set; }

    public double? MedAllw { get; set; }

    public string Sex { get; set; } = null!;

    public string? SpouseName { get; set; }

    public string? MaidenName { get; set; }

    public DateTime? DtExpectedConfirm { get; set; }

    public string HomeAddress { get; set; } = null!;

    public string Marital { get; set; } = null!;

    public string? Idval { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? State { get; set; }

    public string? Lgovt { get; set; }

    public string? AcadQual { get; set; }

    public string? Nok { get; set; }

    public string? Nokaddress { get; set; }

    public string? Nokrel { get; set; }

    public string? Nokphone { get; set; }

    public bool? IsConfirm { get; set; }

    public bool? IsDelete { get; set; }

    public byte[]? EmpPix { get; set; }

    public string? FirstRef { get; set; }

    public string? FirstRefAddress { get; set; }

    public string? SecondRef { get; set; }

    public string? SecondRefAddress { get; set; }

    public string? BioId { get; set; }

    public long Sno { get; set; }
}
