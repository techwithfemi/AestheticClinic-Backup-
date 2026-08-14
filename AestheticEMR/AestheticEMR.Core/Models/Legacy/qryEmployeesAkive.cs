using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryEmployeesAkive
{
    [StringLength(152)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string Surname { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string? OtherName { get; set; }

    [StringLength(50)]
    public string EmpNo { get; set; } = null!;

    [StringLength(200)]
    public string Designation { get; set; } = null!;

    [StringLength(50)]
    public string Dept { get; set; } = null!;

    [StringLength(100)]
    public string? EmpType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? HireDate { get; set; }

    public int? Hireage { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? GrossSal { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? JobDesc { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? homeAddress { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? FirstGrtname { get; set; }

    [StringLength(100)]
    public string EmpCat { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpConfirmationDate { get; set; }

    [StringLength(100)]
    public string? SalaryScale { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? LGovt { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? AcadQual { get; set; }

    [StringLength(50)]
    public string? marital { get; set; }

    public int? EmpAge { get; set; }

    [Column("1stGuarrantorAddress")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? _1stGuarrantorAddress { get; set; }

    [Column("2ndGuarrantorName")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? _2ndGuarrantorName { get; set; }

    [Column("2ndGuarrantorAddress")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? _2ndGuarrantorAddress { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? NOK { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? NOKAddress { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? NOKRel { get; set; }

    [StringLength(50)]
    public string? spouseName { get; set; }

    [StringLength(50)]
    public string? maidenName { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? FirstReferee { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? FirstRefAddress { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? SecondReferee { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? SecondRefAddress { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MedAllw { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? NOKPhone { get; set; }

    [StringLength(50)]
    public string? BioID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? smsCat { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? smsNextDOB { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string? bankid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? bankacctno { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? salgp { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? salcl { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? salst { get; set; }

    public long? SNoID { get; set; }

    [StringLength(50)]
    public string? IDVal { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string OrgID { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string OrgName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? OrgAddress1 { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? OrgAddress2 { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? OrgAddress3 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? OrgLogo { get; set; }
}
