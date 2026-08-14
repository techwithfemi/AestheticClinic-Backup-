using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhSurgery
{
    [StringLength(101)]
    public string? Fullname { get; set; }

    [StringLength(50)]
    public string? pNo { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(50)]
    public string? coyType { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime sDate { get; set; }

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(500)]
    public string? indications { get; set; }

    [StringLength(500)]
    public string? operation { get; set; }

    [StringLength(500)]
    public string? consent { get; set; }

    [StringLength(500)]
    public string? relation { get; set; }

    [StringLength(500)]
    public string? urinalysis { get; set; }

    [StringLength(500)]
    public string? sediments { get; set; }

    [StringLength(500)]
    public string? sugar { get; set; }

    [StringLength(500)]
    public string? acetone { get; set; }

    [StringLength(500)]
    public string? aib { get; set; }

    [StringLength(500)]
    public string? sg { get; set; }

    [StringLength(500)]
    public string? blood { get; set; }

    [StringLength(500)]
    public string? hb { get; set; }

    [StringLength(500)]
    public string? pcv { get; set; }

    [StringLength(500)]
    public string? wbc { get; set; }

    [StringLength(500)]
    public string? wbc_P { get; set; }

    [StringLength(500)]
    public string? wbc_L { get; set; }

    [StringLength(500)]
    public string? wbc_m { get; set; }

    [StringLength(500)]
    public string? wbc_e { get; set; }

    [StringLength(500)]
    public string? wbc_esr { get; set; }

    [StringLength(500)]
    public string? urea_NA { get; set; }

    [StringLength(500)]
    public string? urea { get; set; }

    [StringLength(500)]
    public string? urea_CL { get; set; }

    [StringLength(500)]
    public string? urea_PCO3 { get; set; }

    [StringLength(500)]
    public string? OccultBlood { get; set; }

    [StringLength(500)]
    public string? chestXray { get; set; }

    [StringLength(500)]
    public string? ecg { get; set; }

    [StringLength(500)]
    public string? bloodGroup { get; set; }

    [StringLength(500)]
    public string? surgeon { get; set; }

    [StringLength(500)]
    public string? assistant { get; set; }

    [StringLength(500)]
    public string? anaesthetist { get; set; }

    [StringLength(500)]
    public string? preOPBP { get; set; }

    [StringLength(500)]
    public string? pulse { get; set; }

    [StringLength(500)]
    public string? postOPBP { get; set; }

    [StringLength(500)]
    public string? hgP { get; set; }

    [Column(TypeName = "ntext")]
    public string? findings { get; set; }

    [Column(TypeName = "ntext")]
    public string? prosedure { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    public int ID { get; set; }

    [StringLength(500)]
    public string? hiv { get; set; }

    [StringLength(500)]
    public string ConsultID { get; set; } = null!;
}
