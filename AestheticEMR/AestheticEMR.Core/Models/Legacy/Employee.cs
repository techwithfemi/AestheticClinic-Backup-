using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class Employee
{
    public string empID { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? OtherName { get; set; }

    public string DeptID { get; set; } = null!;

    public string? UnitID { get; set; }

    public string Designation { get; set; } = null!;

    public string EmpCat { get; set; } = null!;

    public string EmpStatus { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public DateTime DOB { get; set; }

    public string? SalaryScale { get; set; }

    public double GrossSal { get; set; }

    public string? NHSNo { get; set; }

    public string? NSITFNo { get; set; }

    public string JobDesc { get; set; } = null!;

    public string FirstGrtname { get; set; } = null!;

    public string? FirstGrtAddress { get; set; }

    public string? SecondGrtName { get; set; }

    public string? SecondGrtAddress { get; set; }

    public double? MedAllw { get; set; }

    public string Sex { get; set; } = null!;

    public string? spouseName { get; set; }

    public string? maidenName { get; set; }

    public DateTime? DtExpectedConfirm { get; set; }

    public string homeAddress { get; set; } = null!;

    public string marital { get; set; } = null!;

    public string? IDVal { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? State { get; set; }

    public string? LGovt { get; set; }

    public string? AcadQual { get; set; }

    public string? NOK { get; set; }

    public string? NOKAddress { get; set; }

    public string? NOKRel { get; set; }

    public string? NOKPhone { get; set; }

    public bool? isConfirm { get; set; }

    public bool? isDelete { get; set; }

    public byte[]? empPix { get; set; }

    public string? FirstRef { get; set; }

    public string? FirstRefAddress { get; set; }

    public string? SecondRef { get; set; }

    public string? SecondRefAddress { get; set; }

    public string? BioID { get; set; }

    public long SNo { get; set; }
}
