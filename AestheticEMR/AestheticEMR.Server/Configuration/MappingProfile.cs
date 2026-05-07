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
                .ReverseMap();

            CreateMap<Order, OrderVM>()
                .ReverseMap();

            CreateMap<AestheticPatient, AestheticPatientVM>()
                .ReverseMap();

            CreateMap<AestheticConsultation, AestheticConsultationVM>()
                .ReverseMap();

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
                .ReverseMap()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != 0));

            CreateMap<DentalImaging, DentalImagingVM>()
                .ReverseMap()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != 0));

            CreateMap<HConsulting, DentalConsultingVM>()
                .ReverseMap()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != 0));
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
