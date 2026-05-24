using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingForPrivateInPatientInvoice
{
    public string BillNo { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public double? Registration { get; set; }

    public double? Consultation { get; set; }

    public double? Medication { get; set; }

    public double? Injections { get; set; }

    public double? Dressing { get; set; }

    public double? AccommodationFood { get; set; }

    public double? AccommodationRoom { get; set; }

    public double? AccommodationSpecialistDiet { get; set; }

    public double? AnaestheticDrugs { get; set; }

    public double? AnaestheticFee { get; set; }

    public double? Antenatal { get; set; }

    public double? Circumcision { get; set; }

    public double? DeliveryAbnormal { get; set; }

    public double? DeliveryAssisted { get; set; }

    public double? DeliveryNormal { get; set; }

    public double? ENTFee { get; set; }

    public double? EarPiercing { get; set; }

    public double? EpisiotomyRepair { get; set; }

    public double? GeneralSurgeonFee { get; set; }

    public double? Immunization { get; set; }

    public double? InHouseSpecialist { get; set; }

    public double? IncisionDrainage { get; set; }

    public double? InfusionBloodTransfusion { get; set; }

    public double? InfusionOralFluid { get; set; }

    public double? InfusionRegular { get; set; }

    public double? InfusionSpecial { get; set; }

    public double? LabBlood { get; set; }

    public double? LabECG { get; set; }

    public double? LabHormonalAssay { get; set; }

    public double? LabMycology { get; set; }

    public double? LabStool { get; set; }

    public double? LabUrine { get; set; }

    public double? LabUssUltrasound { get; set; }

    public double? LabXRayRegular { get; set; }

    public double? LabXRaySpecial { get; set; }

    public double? LabOthers { get; set; }

    public double? ManipulationUnderAnaesth { get; set; }

    public double? ObstetricianGynaecologistFee { get; set; }

    public double? OperationalCharge { get; set; }

    public double? OpthalmologistFee { get; set; }

    public double? OrthopaedicSurgeonFee { get; set; }

    public double? Phototherapy { get; set; }

    public double? PhysicianPaediatricianFee { get; set; }

    public double? PhysiotherapistFee { get; set; }

    public double? PlasterOfParis { get; set; }

    public double? DentalSurgeon { get; set; }

    public double? CervicalLaceration { get; set; }

    public double? SpecialDrugs { get; set; }

    public double? SpecialTreatment { get; set; }

    public double? SurgeonsFee { get; set; }

    public double? Suturing { get; set; }

    public double? Telephone { get; set; }

    public double? Television { get; set; }

    public double? TheatreUse { get; set; }

    public double? Video { get; set; }
}
