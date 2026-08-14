using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("ChemPathologyOLD")]
public partial class ChemPathologyOLD
{
    public long ID { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(50)]
    public string? SODIUM { get; set; }

    [StringLength(50)]
    public string? POTASS { get; set; }

    [StringLength(50)]
    public string? BICARB { get; set; }

    [StringLength(50)]
    public string? CHLORIDE { get; set; }

    [StringLength(50)]
    public string? UREA { get; set; }

    [StringLength(50)]
    public string? SGOT { get; set; }

    [StringLength(50)]
    public string? SGPT { get; set; }

    [StringLength(50)]
    public string? ALKPHOS { get; set; }

    [StringLength(50)]
    public string? BILIRUBIN { get; set; }

    [StringLength(50)]
    public string? FAST_BLOOD_SUGAR { get; set; }

    [StringLength(50)]
    public string? RAND_BLOOD_SUGAR { get; set; }

    [StringLength(50)]
    public string? GLUC_TOL_TEST { get; set; }

    [StringLength(50)]
    public string? ACIDPHOS { get; set; }

    [StringLength(50)]
    public string? GAMMA_GT { get; set; }

    [StringLength(50)]
    public string? CPK { get; set; }

    [StringLength(50)]
    public string? HIV { get; set; }

    [StringLength(50)]
    public string? VDR { get; set; }

    [StringLength(50)]
    public string? HEPATITIS { get; set; }

    [StringLength(50)]
    public string? CHLAMYDIA { get; set; }

    [StringLength(50)]
    public string? TOXOPLAMA { get; set; }

    [StringLength(50)]
    public string? COMBS { get; set; }

    [StringLength(50)]
    public string? ASO_TITRE { get; set; }

    [StringLength(50)]
    public string? CALCIUM { get; set; }

    [StringLength(50)]
    public string? PHOS { get; set; }

    [StringLength(50)]
    public string? URIC_ACID { get; set; }

    [StringLength(50)]
    public string? CREATININE { get; set; }

    [StringLength(50)]
    public string? AMYLASE { get; set; }

    [StringLength(50)]
    public string? LDH { get; set; }

    [StringLength(50)]
    public string? TOT_PROTEIN { get; set; }

    [StringLength(50)]
    public string? ALBUMIN { get; set; }

    [StringLength(50)]
    public string? SERUM_IRON { get; set; }

    [StringLength(50)]
    public string? TIBC { get; set; }

    [StringLength(50)]
    public string? CHOLEST { get; set; }

    [StringLength(50)]
    public string? TRIGLYCE { get; set; }

    [StringLength(50)]
    public string? CSF_PROTEIN { get; set; }

    [StringLength(50)]
    public string? CSF_SUGAR { get; set; }

    [StringLength(50)]
    public string? CSF_CHLOR { get; set; }

    [StringLength(50)]
    public string? VMA { get; set; }

    [StringLength(50)]
    public string? HB_AIC { get; set; }

    [StringLength(50)]
    public string? PREG_TEST_BLOOD { get; set; }

    [StringLength(50)]
    public string? PREG_TEST_URINE { get; set; }

    [StringLength(50)]
    public string? COLOR { get; set; }

    [StringLength(50)]
    public string? PH { get; set; }

    [StringLength(50)]
    public string? UR_PROTEIN { get; set; }

    [StringLength(50)]
    public string? UR_SUGAR { get; set; }

    [StringLength(50)]
    public string? UR_KETONES { get; set; }

    [StringLength(50)]
    public string? UR_BLOOD { get; set; }

    [StringLength(50)]
    public string? UR_BILIRUBIN { get; set; }

    [StringLength(50)]
    public string? UR_UROBILI { get; set; }

    [StringLength(50)]
    public string? UR_SG { get; set; }

    [StringLength(50)]
    public string? UR_BILE { get; set; }

    [StringLength(50)]
    public string? UR_LEUCO { get; set; }

    [StringLength(50)]
    public string? UR_NITRITES { get; set; }
}
