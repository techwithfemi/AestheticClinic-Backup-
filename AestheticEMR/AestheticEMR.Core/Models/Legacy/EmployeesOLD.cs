using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmployeesOLD")]
public partial class EmployeesOLD
{
    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [StringLength(65)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(116)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string Designation { get; set; } = null!;

    [StringLength(50)]
    public string DeptID { get; set; } = null!;

    [StringLength(50)]
    public string? EmpStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? HireDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DOB { get; set; }

    public double? GrossSal { get; set; }

    [StringLength(50)]
    public string? NHSNo { get; set; }

    [StringLength(50)]
    public string? NSITFNo { get; set; }

    [StringLength(3000)]
    public string? JobDesc { get; set; }

    [StringLength(50)]
    public string? FirstGrtname { get; set; }

    [StringLength(250)]
    public string? FirstGrtAddress { get; set; }

    [StringLength(50)]
    public string? SecondGrtName { get; set; }

    [StringLength(250)]
    public string? SecondGrtAddress { get; set; }

    [StringLength(50)]
    public string? EmpCat { get; set; }

    public double? MedAllw { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(50)]
    public string? spouseName { get; set; }

    [StringLength(50)]
    public string? maidenName { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DtExpectedConfirm { get; set; }

    [StringLength(250)]
    public string? homeAddress { get; set; }

    [StringLength(50)]
    public string? marital { get; set; }

    [StringLength(50)]
    public string? IDVal { get; set; }

    [StringLength(50)]
    public string? Authorizer { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(150)]
    public string? Email { get; set; }
}
