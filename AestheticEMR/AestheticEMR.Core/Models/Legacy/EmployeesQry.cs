using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class EmployeesQry
{
    [StringLength(50)]
    public string empID { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string? OtherName { get; set; }

    [StringLength(50)]
    public string DeptID { get; set; } = null!;

    [StringLength(50)]
    public string? UnitID { get; set; }

    [StringLength(50)]
    public string Designation { get; set; } = null!;

    [StringLength(50)]
    public string EmpCat { get; set; } = null!;

    [StringLength(50)]
    public string EmpStatus { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime HireDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DOB { get; set; }

    [StringLength(100)]
    public string? SalaryScale { get; set; }

    public double GrossSal { get; set; }

    [StringLength(50)]
    public string? NHSNo { get; set; }

    [StringLength(50)]
    public string? NSITFNo { get; set; }

    [StringLength(3500)]
    public string JobDesc { get; set; } = null!;

    [StringLength(50)]
    public string FirstGrtname { get; set; } = null!;

    [StringLength(250)]
    public string? FirstGrtAddress { get; set; }

    [StringLength(50)]
    public string? SecondGrtName { get; set; }

    [StringLength(250)]
    public string? SecondGrtAddress { get; set; }

    public double? MedAllw { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    public string? spouseName { get; set; }

    [StringLength(50)]
    public string? maidenName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtExpectedConfirm { get; set; }

    [StringLength(250)]
    public string homeAddress { get; set; } = null!;

    [StringLength(50)]
    public string marital { get; set; } = null!;

    [StringLength(50)]
    public string? IDVal { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(50)]
    public string? LGovt { get; set; }

    [StringLength(250)]
    public string? AcadQual { get; set; }

    [StringLength(250)]
    public string? NOK { get; set; }

    [StringLength(250)]
    public string? NOKAddress { get; set; }

    [StringLength(50)]
    public string? NOKRel { get; set; }

    [StringLength(50)]
    public string? NOKPhone { get; set; }

    public bool? isConfirm { get; set; }

    public bool? isDelete { get; set; }

    [Column(TypeName = "image")]
    public byte[]? empPix { get; set; }

    [StringLength(50)]
    public string? FirstRef { get; set; }

    [StringLength(250)]
    public string? FirstRefAddress { get; set; }

    [StringLength(50)]
    public string? SecondRef { get; set; }

    [StringLength(250)]
    public string? SecondRefAddress { get; set; }

    [StringLength(52)]
    public string? BioID { get; set; }

    public long SNo { get; set; }
}
