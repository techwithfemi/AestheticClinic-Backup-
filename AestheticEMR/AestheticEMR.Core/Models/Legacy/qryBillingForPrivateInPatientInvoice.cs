using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingForPrivateInPatientInvoice
{
    [StringLength(50)]
    public string BillNo { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(101)]
    public string fullname { get; set; } = null!;

    [StringLength(250)]
    public string diagnosis { get; set; } = null!;

    public double? REGISTRATION { get; set; }

    public double? CONSULTATION { get; set; }

    public double? MEDICATION { get; set; }

    public double? INJECTIONS { get; set; }

    public double? DRESSING { get; set; }

    public double? ACCOMMODATION_FOOD { get; set; }

    public double? ACCOMMODATION_ROOM { get; set; }

    public double? ACCOMMODATION_SPECIALIST_DIET { get; set; }

    public double? ANAESTHETIC_DRUGS { get; set; }

    public double? ANAESTHETIC_FEE { get; set; }

    public double? ANTENATAL { get; set; }

    public double? CIRCUMCISION { get; set; }

    public double? DELIVERY_ABNORMAL { get; set; }

    public double? DELIVERY_ASSISTED { get; set; }

    public double? DELIVERY_NORMAL { get; set; }

    [Column("E.N.T_FEE")]
    public double? E_N_T_FEE { get; set; }

    public double? EAR_PIERCING { get; set; }

    [Column("EPISIOTOMY_&_REPAIR")]
    public double? EPISIOTOMY___REPAIR { get; set; }

    public double? GENERAL_SURGEON_FEE { get; set; }

    public double? IMMUNIZATION { get; set; }

    public double? IN_HOUSE_SPECIALIST { get; set; }

    [Column("INCISION/DRAINAGE")]
    public double? INCISION_DRAINAGE { get; set; }

    public double? INFUSION_BLOOD_TRANSFUSION { get; set; }

    public double? INFUSION_ORAL_FLUID { get; set; }

    public double? INFUSION_REGULAR { get; set; }

    public double? INFUSION_SPECIAL { get; set; }

    public double? LAB_BLOOD { get; set; }

    [Column("LAB_E.C.G")]
    public double? LAB_E_C_G { get; set; }

    public double? LAB_HORMONAL_ASSAY { get; set; }

    public double? LAB_MYCOLOGY { get; set; }

    public double? LAB_STOOL { get; set; }

    public double? LAB_URINE { get; set; }

    public double? LAB_USS_ULTRASOUND { get; set; }

    [Column("LAB_X-RAY_REGULAR")]
    public double? LAB_X_RAY_REGULAR { get; set; }

    [Column("LAB_X-RAY_SPECIAL")]
    public double? LAB_X_RAY_SPECIAL { get; set; }

    public double? LAB_OTHERS { get; set; }

    public double? MANIPULATION_UNDER_ANAESTH { get; set; }

    [Column("OBSTETRICIAN/GYNAECOLOGIST_FEE")]
    public double? OBSTETRICIAN_GYNAECOLOGIST_FEE { get; set; }

    public double? OPERATIONAL_CHARGE { get; set; }

    public double? OPTHALMOLOGIST_FEE { get; set; }

    public double? ORTHOPAEDIC_SURGEON_FEE { get; set; }

    public double? PHOTOTHERAPY { get; set; }

    [Column("PHYSICIAN/PAEDIATRICIAN_FEE")]
    public double? PHYSICIAN_PAEDIATRICIAN_FEE { get; set; }

    public double? PHYSIOTHERAPIST_FEE { get; set; }

    public double? PLASTER_OF_PARIS { get; set; }

    public double? DENTAL_SURGEON { get; set; }

    public double? CERVICAL_LACERATION { get; set; }

    public double? SPECIAL_DRUGS { get; set; }

    public double? SPECIAL_TREATMENT { get; set; }

    public double? SURGEONS_FEE { get; set; }

    public double? SUTURING { get; set; }

    public double? TELEPHONE { get; set; }

    public double? TELEVISION { get; set; }

    public double? THEATRE_USE { get; set; }

    public double? VIDEO { get; set; }
}
