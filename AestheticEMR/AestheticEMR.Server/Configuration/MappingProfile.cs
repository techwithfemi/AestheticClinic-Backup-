// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models.Account;
using AestheticEMR.Core.Models.Aesthetic;
using AestheticEMR.Core.Models.Dental;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Models.Shop;
using AestheticEMR.Core.Services.Account;
using AestheticEMR.Server.ViewModels.Account;
using AestheticEMR.Server.ViewModels.Aesthetic;
using AestheticEMR.Server.ViewModels.Dental;
using AestheticEMR.Server.ViewModels.Legacy;
using AestheticEMR.Server.ViewModels.Shop;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AestheticEMR.Server.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserVM>()
                   .ForMember(d => d.UserPhotoBase64, map => map.MapFrom(s => ToDataUrl(s.UserPhoto)))
                   .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserVM, ApplicationUser>()
                .ForMember(d => d.UserPhoto, map => map.MapFrom(s => FromBase64(s.UserPhotoBase64)))
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<ApplicationUser, UserEditVM>()
                .ForMember(d => d.UserPhotoBase64, map => map.MapFrom(s => ToDataUrl(s.UserPhoto)))
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserEditVM, ApplicationUser>()
                .ForMember(d => d.UserPhoto, map => map.MapFrom(s => FromBase64(s.UserPhotoBase64)))
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<ApplicationUser, UserPatchVM>()
                .ReverseMap();

            CreateMap<ApplicationRole, RoleVM>()
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.MapFrom(s => s.Users != null ? s.Users.Count : 0))
                .ReverseMap();
            CreateMap<RoleVM, ApplicationRole>()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<IdentityRoleClaim<string>, ClaimVM>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<ApplicationPermission, PermissionVM>()
                .ReverseMap();

            CreateMap<IdentityRoleClaim<string>, PermissionVM>()
                .ConvertUsing(s => ((PermissionVM)ApplicationPermissions.GetPermissionByValue(s.ClaimValue))!);

            CreateMap<Customer, CustomerVM>()
                .ReverseMap();

            CreateMap<Product, ProductVM>()
                .ForMember(d => d.ProductCategoryName, map => map.MapFrom(s => s.ProductCategory != null ? s.ProductCategory.Name : null));

            CreateMap<ProductStockReport, ProductStockReportVM>()
                .ForMember(d => d.ProductName, map => map.MapFrom(s => s.Product != null ? s.Product.Name : null));

            CreateMap<ProductBatch, ProductBatchVM>()
                .ForMember(d => d.ProductName, map => map.MapFrom(s => s.Product != null ? s.Product.Name : null));
            CreateMap<ProductBatchEditVM, ProductBatch>()
                .ForMember(d => d.Product, map => map.Ignore())
                .ForMember(d => d.ProcedureUsages, map => map.Ignore());

            CreateMap<ProcedureProductUsage, ProcedureProductUsageVM>()
                .ForMember(d => d.ProductName, map => map.MapFrom(s => s.Product != null ? s.Product.Name : null))
                .ForMember(d => d.BatchNumber, map => map.MapFrom(s => s.ProductBatch != null ? s.ProductBatch.BatchNumber : null));
            CreateMap<ProcedureProductUsageEditVM, ProcedureProductUsage>()
                .ForMember(d => d.Product, map => map.Ignore())
                .ForMember(d => d.ProductBatch, map => map.Ignore())
                .ForMember(d => d.Consultation, map => map.Ignore());

            CreateMap<ProductEditVM, Product>()
                .ForMember(d => d.ProductCategory, map => map.Ignore())
                .ForMember(d => d.Parent, map => map.Ignore())
                .ForMember(d => d.Children, map => map.Ignore())
                .ForMember(d => d.OrderDetails, map => map.Ignore())
                .ForMember(d => d.Batches, map => map.Ignore())
                .ForMember(d => d.ProcedureUsages, map => map.Ignore());

            CreateMap<ProductCategory, ProductCategoryVM>();
            CreateMap<ProductCategoryEditVM, ProductCategory>();

            CreateMap<Order, OrderVM>()
                .ReverseMap();

            CreateMap<AestheticPatient, AestheticPatientVM>()
                .ReverseMap();

            CreateMap<AestheticConsultation, AestheticConsultationVM>()
                .ReverseMap();

            CreateMap<AestheticFollowUp, AestheticFollowUpVM>()
                .ForMember(d => d.PatientId, map => map.MapFrom(s => s.Consultation.PatientId))
                .ForMember(d => d.PatientName, map => map.MapFrom(s => s.Consultation.Patient != null ? $"{s.Consultation.Patient.FirstName} {s.Consultation.Patient.LastName}" : null));

            CreateMap<ScheduleAestheticFollowUpVM, AestheticFollowUp>();
            CreateMap<CompleteAestheticFollowUpVM, AestheticFollowUp>();

            CreateMap<ProcedureRevenueMetric, ProcedureRevenueMetricVM>();
            CreateMap<ProductUsageMetric, ProductUsageMetricVM>();
            CreateMap<ComplicationRateMetric, ComplicationRateMetricVM>();
            CreateMap<PatientRetentionMetric, PatientRetentionMetricVM>();
            CreateMap<BeforeAfterOutcomeMetric, BeforeAfterOutcomeMetricVM>();

            CreateMap<AestheticConsentTemplate, AestheticConsentTemplateVM>()
                .ReverseMap();

            CreateMap<AestheticSignedConsent, AestheticSignedConsentVM>()
                .ForMember(d => d.SignatureImageBase64, map => map.MapFrom(s => ToDataUrl(s.SignatureImage)))
                .ForMember(d => d.SignatureImagePath, map => map.MapFrom(s => s.SignatureImagePath))
                .ReverseMap()
                .ForMember(d => d.SignatureImage, map => map.MapFrom(s => FromBase64(s.SignatureImageBase64)))
                .ForMember(d => d.SignatureImagePath, map => map.MapFrom(s => s.SignatureImagePath));

            CreateMap<AestheticConsentStatus, AestheticConsentStatusVM>()
                .ForMember(d => d.CanSign, map => map.MapFrom(s => s.CanSign));

            CreateMap<AestheticPhoto, AestheticPhotoVM>()
                .ForMember(d => d.Url, map => map.MapFrom(s => s.FilePath))
                .ForMember(d => d.ThumbnailUrl, map => map.MapFrom(s => s.FilePath))
                .ForMember(d => d.CreatedDate, map => map.MapFrom(s => s.CreatedDate))
                .ReverseMap()
                .ForMember(d => d.FilePath, map => map.MapFrom(s => s.Url));

            CreateMap<HRetainership, HRetainershipVM>()
                .ReverseMap();

            CreateMap<hServiceNHI, ServiceTariffVM>()
                .ForMember(d => d.Sno, map => map.MapFrom(s => s.SNO))
                .ForMember(d => d.CoyId, map => map.MapFrom(s => s.Company));

            CreateMap<VwServiceNhi, ServiceTariffVM>()
                .ForMember(d => d.CoyId, map => map.MapFrom(s => s.CoyId))
                .ForMember(d => d.CoyName, map => map.MapFrom(s => s.Company))
                .ForMember(d => d.Company, map => map.MapFrom(s => s.CoyId));

            CreateMap<ServiceTariffVM, hServiceNHI>()
                .ForMember(d => d.SNO, map => map.MapFrom(s => s.Sno))
                .ForMember(d => d.Company, map => map.MapFrom(s => !string.IsNullOrWhiteSpace(s.Company) ? s.Company : s.CoyId));

            CreateMap<VwCoyAndNhi, TariffCompanyVM>();

            CreateMap<HRecord, AttendanceVM>()
                .ReverseMap()
                .ForMember(d => d.ConsultId, map => map.Ignore())
                .ForMember(d => d.RecId, map => map.Ignore());

            CreateMap<QryhvisitsForToday, QryhvisitsForTodayVM>();

            CreateMap<hAppointment, AppointmentVM>()
                .ForMember(d => d.Id, map => map.MapFrom(s => s.ID))
                .ForMember(d => d.Pno, map => map.MapFrom(s => s.pno))
                .ForMember(d => d.ClinicType, map => map.MapFrom(s => s.clinicType))
                .ForMember(d => d.Remarks, map => map.MapFrom(s => s.remarks))
                .ReverseMap()
                .ForMember(d => d.ID, map => map.MapFrom(s => s.Id))
                .ForMember(d => d.pno, map => map.MapFrom(s => s.Pno))
                .ForMember(d => d.clinicType, map => map.MapFrom(s => s.ClinicType))
                .ForMember(d => d.remarks, map => map.MapFrom(s => s.Remarks));

            CreateMap<HPatient, HPatientVM>()
                .ForMember(d => d.PatPixBase64, map => map.MapFrom(s =>
                    s.PatPix != null && s.PatPix.Length > 0
                        ? Convert.ToBase64String(s.PatPix)
                        : null))
                .ReverseMap()
                .ForMember(d => d.Pno, map => map.Ignore())
                .ForMember(d => d.PatPix, map => map.MapFrom(s =>
                    !string.IsNullOrWhiteSpace(s.PatPixBase64)
                        ? Convert.FromBase64String(StripBase64Prefix(s.PatPixBase64))
                        : null));

            CreateMap<HDentalTreat, DentalChartVM>()
                .ForMember(d => d.PatientName, map => map.Ignore())
                .ForMember(d => d.TeethStatus, map => map.Ignore())
                .ForMember(d => d.Orthodontics, map => map.Ignore())
                // Map new clinical findings fields
                .ForMember(d => d.InflammationOfGingiva, map => map.MapFrom(s => s.InflammationOfGingiva))
                .ForMember(d => d.PresenceOfDebris, map => map.MapFrom(s => s.PresenceOfDebris))
                .ForMember(d => d.PresenceOfCalculus, map => map.MapFrom(s => s.PresenceOfCalculus))
                .ForMember(d => d.PresenceOfStains, map => map.MapFrom(s => s.PresenceOfStains))
                .ForMember(d => d.UnderOrthodonticTreatment, map => map.MapFrom(s => s.UnderOrthodonticTreatment))
                .ForMember(d => d.OtherClinicalFindings, map => map.MapFrom(s => s.OtherClinicalFindings))
                .ReverseMap()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != 0))
                .ForMember(d => d.TeethStatusJson, map => map.Ignore())
                .ForMember(d => d.OrthodonticsJson, map => map.Ignore())
                // Map new clinical findings fields
                .ForMember(d => d.InflammationOfGingiva, map => map.MapFrom(s => s.InflammationOfGingiva))
                .ForMember(d => d.PresenceOfDebris, map => map.MapFrom(s => s.PresenceOfDebris))
                .ForMember(d => d.PresenceOfCalculus, map => map.MapFrom(s => s.PresenceOfCalculus))
                .ForMember(d => d.PresenceOfStains, map => map.MapFrom(s => s.PresenceOfStains))
                .ForMember(d => d.UnderOrthodonticTreatment, map => map.MapFrom(s => s.UnderOrthodonticTreatment))
                .ForMember(d => d.OtherClinicalFindings, map => map.MapFrom(s => s.OtherClinicalFindings));

            CreateMap<DentalImaging, DentalImagingVM>()
                .ReverseMap()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != 0));

            CreateMap<HConsulting, DentalConsultingVM>()
                .ForMember(d => d.ClientCat, map => map.MapFrom(s => s.ClientCat))
                .ReverseMap()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != 0))
                // ClientCat is optional in VM; fall back to "PRIVATE" when null/empty
                .ForMember(d => d.ClientCat, map => map.MapFrom(s => string.IsNullOrWhiteSpace(s.ClientCat) ? "PRIVATE" : s.ClientCat));

            CreateMap<Billing, BillingVM>()
                .ForMember(d => d.BillNo, map => map.MapFrom(s => s.billNO))
                .ForMember(d => d.BDate, map => map.MapFrom(s => s.bDate))
                .ForMember(d => d.PNo, map => map.MapFrom(s => s.pNo))
                .ForMember(d => d.ClientID, map => map.MapFrom(s => s.clientID))
                .ForMember(d => d.Tax, map => map.MapFrom(s => s.Tax))
                .ForMember(d => d.BillType, map => map.MapFrom(s => s.billType))
                .ForMember(d => d.IsPaid, map => map.MapFrom(s => s.isPaid))
                .ForMember(d => d.Details, map => map.Ignore());

            CreateMap<BillingVM, Billing>()
                .ForMember(d => d.billNO, map => map.MapFrom(s => s.BillNo))
                .ForMember(d => d.bDate, map => map.MapFrom(s => s.BDate))
                .ForMember(d => d.pNo, map => map.MapFrom(s => s.PNo))
                .ForMember(d => d.clientID, map => map.MapFrom(s => s.ClientID))
                .ForMember(d => d.Tax, map => map.MapFrom(s => s.Tax))
                .ForMember(d => d.billType, map => map.MapFrom(s => s.BillType))
                .ForMember(d => d.isPaid, map => map.MapFrom(s => s.IsPaid))
                .ForMember(d => d.ID, opt => opt.Ignore()); // Prevent mapping ID

            CreateMap<BillingDetail, BillingDetailVM>()
                .ForMember(d => d.TranID, map => map.MapFrom(s => s.TranID))
                .ForMember(d => d.DrgName, map => map.MapFrom(s => s.drgName))
                .ForMember(d => d.BillType, map => map.MapFrom(s => s.billType))
                .ForMember(d => d.ConID, map => map.MapFrom(s => s.conID))
                .ForMember(d => d.RevenueType, map => map.MapFrom(s => s.revType))
                .ForMember(d => d.Category, map => map.MapFrom(s => s.Category))
                .ForMember(d => d.RevClinic, map => map.MapFrom(s => s.RevClinic))
                .ForMember(d => d.BillTo, map => map.MapFrom(s => s.BillTo))
                .ForMember(d => d.CoyName, map => map.MapFrom(s => s.CoyName));

            CreateMap<BillingDetailVM, BillingDetail>()
                .ForMember(d => d.TranID, map => map.MapFrom(s => s.TranID))
                .ForMember(d => d.drgName, map => map.MapFrom(s => s.DrgName))
                .ForMember(d => d.billType, map => map.MapFrom(s => s.BillType))
                .ForMember(d => d.conID, map => map.MapFrom(s => s.ConID))
                .ForMember(d => d.revType, map => map.MapFrom(s => s.RevenueType))
                .ForMember(d => d.Category, map => map.MapFrom(s => s.Category))
                .ForMember(d => d.RevClinic, map => map.MapFrom(s => s.RevClinic))
                .ForMember(d => d.BillTo, map => map.MapFrom(s => s.BillTo))
                .ForMember(d => d.CoyName, map => map.MapFrom(s => s.CoyName));
        }

        private static string StripBase64Prefix(string base64)
        {
            // Handles "data:image/jpeg;base64,..." or plain base64
            var idx = base64.IndexOf(',');
            return idx >= 0 ? base64[(idx + 1)..] : base64;
        }

        private static string? ToDataUrl(byte[]? bytes)
        {
            return bytes != null && bytes.Length > 0
                ? $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}"
                : null;
        }

        private static byte[]? FromBase64(string? base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return null;

            try
            {
                return Convert.FromBase64String(StripBase64Prefix(base64));
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
