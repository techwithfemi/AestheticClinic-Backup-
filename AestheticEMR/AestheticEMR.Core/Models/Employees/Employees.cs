namespace AestheticEMR.Core.Models.Employees;

public partial class Employees
{
    public string EmpId { get; set; } = null!; // pri key

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? OtherName { get; set; } = null!;

    public string? DeptId { get; set; } // foreign key empdepartment

    public string? UnitId { get; set; } =null!; 

    public string? Designation { get; set; } // foreign key signation

    public string? EmpCat { get; set; } // foreign key signation

    public string? EmpStatus { get; set; } // foreign key signation

    public DateTime? HireDate { get; set; } = null!;

    public DateTime? Dob { get; set; } // date of birth

    public string? SalaryScale { get; set; } = null!;       

    public decimal? GrossSal { get; set; } = 0.00m;

    public string? NhsNo { get; set; } = null!;

    public string? NsitfNo { get; set; } = null!;

    public string? JobDesc { get; set; } = null!;

    public string? FirstGrtName { get; set; } = null!;

    public string? FirstGrtAddress { get; set; } = null!;

    public string? SecondGrtName { get; set; } = null!;

    public string? SecondGrtAddress { get; set; } = null!;

    public decimal? MedAllw { get; set; }

    public string? Sex { get; set; } = null!;
}
