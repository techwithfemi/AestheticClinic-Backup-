using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("Akive")]
public partial class Akive
{
    [StringLength(50)]
    public string empID { get; set; } = null!;

    [StringLength(50)]
    public string? BioID { get; set; }

    public long? SNo { get; set; }

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
    public DateTime? HireDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(100)]
    public string? SalaryScale { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? GrossSal { get; set; }

    [StringLength(50)]
    public string? NHSNo { get; set; }

    [StringLength(50)]
    public string? NSITFNo { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? JobDesc { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? FirstGrtname { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? FirstGrtAddress { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? SecondGrtName { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? SecondGrtAddress { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MedAllw { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(50)]
    public string? spouseName { get; set; }

    [StringLength(50)]
    public string? maidenName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtExpectedConfirm { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? homeAddress { get; set; }

    [StringLength(50)]
    public string? marital { get; set; }

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

    [StringLength(8000)]
    [Unicode(false)]
    public string? AcadQual { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? NOK { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? NOKAddress { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? NOKRel { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? NOKPhone { get; set; }

    public bool? isConfirm { get; set; }

    public bool? isDelete { get; set; }

    public byte[]? empPix { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? FirstRef { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? FirstRefAddress { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? SecondRef { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? SecondRefAddress { get; set; }

    [StringLength(50)]
    public string? Authorizer { get; set; }

    [StringLength(50)]
    public string? PayPoint { get; set; }

    [StringLength(50)]
    public string? Location { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? division { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? rankseq { get; set; }

    [StringLength(50)]
    public string? middlename { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? salgp { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? salcl { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? salst { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? saleffdate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? enlistdate { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? pensioncode { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string? bankid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? bankacctno { get; set; }

    public int? paystatus { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? paypoint1 { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? corpscode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? birthdate { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string? bloodgroup { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? maritalsta { get; set; }

    public int? noofchild { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? statecode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? lga { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? hometown { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? homeaddr01 { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? homeaddr02 { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? nextofkin { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? startdate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? enddate { get; set; }

    public double? grossytd { get; set; }

    public double? taxableytd { get; set; }

    public double? netytd { get; set; }

    public double? taxytd { get; set; }

    public double? freepay { get; set; }

    public bool? in_qtrs { get; set; }

    [Column(TypeName = "decimal(4, 0)")]
    public decimal? analcode { get; set; }

    public int? jumper { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? wotrade { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? nationalid { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? picloc { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? thumbloc { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? signloc { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? digital { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? nextofkin1address { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? nextofkin1relation { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? nextofkin2address { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? nextofkin2relation { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? thumbprintpath { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? signaturepath { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? nok2 { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? nhscode { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? nhscodeold { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? nhiscode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? pencomPIN { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? pencomName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? pfaAcctNo { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? pfaBankName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? glevel { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? smsCat { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? smsNextDOB { get; set; }

    public bool? isLeft { get; set; }
}
