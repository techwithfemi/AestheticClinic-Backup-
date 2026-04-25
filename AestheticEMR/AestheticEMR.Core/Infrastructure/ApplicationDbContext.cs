// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models;
using AestheticEMR.Core.Models.Account;
using AestheticEMR.Core.Models.Aesthetic;
using AestheticEMR.Core.Models.Shop;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AestheticEMR.Core.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IUserIdAccessor _userIdAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserIdAccessor userIdAccessor)
            : base(options)
        {
            _userIdAccessor = userIdAccessor;
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<HConsulting> HConsultings { get; set; }
        public DbSet<HConsultingItem> HConsultingItems { get; set; }
        public DbSet<HDental> HDentals { get; set; }
        public DbSet<HDentalTreat> HDentalTreats { get; set; }
        public DbSet<ClinicType> ClinicTypes { get; set; }
        public DbSet<HClinicPurpose> HClinicPurposes { get; set; }

        public DbSet<HPatient> HPatients { get; set; }
        public DbSet<HRecord> HRecords { get; set; }
        public virtual DbSet<HRetainership> HRetainerships { get; set; }
        public DbSet<BillAccum> BillAccums { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<BillingDetail> BillingDetails { get; set; }
        public DbSet<BillsForClient> BillsForClients { get; set; }
        public DbSet<DrugNHISListEntered> DrugNHISListEntereds { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentTypeDetail> PaymentTypeDetails { get; set; }
        public DbSet<hAppointment> hAppointments { get; set; }
        public DbSet<hReferal> hReferals { get; set; }
        public DbSet<hRevenueType> hRevenueTypes { get; set; }
        public DbSet<hService> hServices { get; set; }
        public DbSet<hServiceNHI> hServiceNHIs { get; set; }
        public DbSet<vwEmpCashier> vwEmpCashiers { get; set; }
        public DbSet<vwEmpName> vwEmpNames { get; set; }
        public DbSet<vwEmployee> vwEmployees { get; set; }
        public DbSet<AestheticPatient> AestheticPatients { get; set; }
        public DbSet<AestheticConsultation> AestheticConsultations { get; set; }
        public DbSet<AestheticPhoto> AestheticPhotos { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            const string priceDecimalType = "decimal(18,2)";
            const string tablePrefix = "App";
            const string appNameSql = "(app_name())";
            const string hostNameSql = "(host_name())";
            const string dateTimeSql = "datetime";
            const string decimalType = "decimal(18, 2)";
            const string getDateSql = "(CONVERT([varchar](10),getdate(),(23)))";
            const string getTimeSql = "(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))";
            const string consultIdColumn = "consultID";

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Claims)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Roles)
                .WithOne()
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>()
                .HasMany(r => r.Claims)
                .WithOne()
                .HasForeignKey(c => c.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationRole>()
                .HasMany(r => r.Users)
                .WithOne()
                .HasForeignKey(r => r.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Customer>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Customer>().HasIndex(c => c.Name);
            builder.Entity<Customer>().Property(c => c.Email).HasMaxLength(100);
            builder.Entity<Customer>().Property(c => c.PhoneNumber).IsUnicode(false).HasMaxLength(30);
            builder.Entity<Customer>().Property(c => c.City).HasMaxLength(50);
            builder.Entity<Customer>().ToTable($"{tablePrefix}{nameof(Customers)}");

            builder.Entity<ProductCategory>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<ProductCategory>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<ProductCategory>().ToTable($"{tablePrefix}{nameof(ProductCategories)}");

            builder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Product>().HasIndex(p => p.Name);
            builder.Entity<Product>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<Product>().Property(p => p.Icon).IsUnicode(false).HasMaxLength(256);
            builder.Entity<Product>().HasOne(p => p.Parent).WithMany(p => p.Children).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Product>().Property(p => p.BuyingPrice).HasColumnType(priceDecimalType);
            builder.Entity<Product>().Property(p => p.SellingPrice).HasColumnType(priceDecimalType);
            builder.Entity<Product>().ToTable($"{tablePrefix}{nameof(Products)}");

            builder.Entity<Order>().Property(o => o.Comments).HasMaxLength(500);
            builder.Entity<Order>().Property(p => p.Discount).HasColumnType(priceDecimalType);
            builder.Entity<Order>().ToTable($"{tablePrefix}{nameof(Orders)}");

            builder.Entity<OrderDetail>().Property(p => p.UnitPrice).HasColumnType(priceDecimalType);
            builder.Entity<OrderDetail>().Property(p => p.Discount).HasColumnType(priceDecimalType);
            builder.Entity<OrderDetail>().ToTable($"{tablePrefix}{nameof(OrderDetails)}");

            builder.Entity<HConsulting>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("hConsulting");

                entity.HasIndex(e => e.CDate, "IX_hConsulting");

                entity.HasIndex(e => e.CDate, "idxCdate");

                entity.HasIndex(e => e.PNo, "idxPno");

                entity.HasIndex(e => e.CDate, "idx_CDate");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(app_name())");
                entity.Property(e => e.AttendedTo)
                    .HasDefaultValue(false)
                    .HasColumnName("attendedTo");
                entity.Property(e => e.AttendedToByHmo)
                    .HasDefaultValue(false)
                    .HasColumnName("AttendedToByHMO");
                entity.Property(e => e.AttendedToByPharm)
                    .HasDefaultValue(false)
                    .HasColumnName("attendedToByPharm");
                entity.Property(e => e.AttendedtoByLab).HasDefaultValue(false);
                entity.Property(e => e.BillRemarks)
                    .HasMaxLength(3000)
                    .IsUnicode(false);
                entity.Property(e => e.CDate)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("cDate");
                entity.Property(e => e.CTime)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("cTime");
                entity.Property(e => e.ClientCat)
                    .HasMaxLength(50)
                    .HasColumnName("clientCat");
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql);
                entity.Property(e => e.Clinic)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("clinic");
                entity.Property(e => e.ClinicRemarks).IsUnicode(false);
                entity.Property(e => e.Complaints)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("complaints");
                entity.Property(e => e.ConsultId)
                    .HasMaxLength(50)
                    .HasColumnName(consultIdColumn);
                entity.Property(e => e.DentHist)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("dentHist");
                entity.Property(e => e.Diagnosis)
                    .IsUnicode(false)
                    .HasColumnName("diagnosis");
                entity.Property(e => e.DiffDiagnosis)
                    .IsUnicode(false)
                    .HasColumnName("diffDiagnosis");
                entity.Property(e => e.DrugHx)
                    .HasMaxLength(3000)
                    .IsUnicode(false);
                entity.Property(e => e.EditDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.EditTime).HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql(getTimeSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.ExtraOralExam)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("extraOralExam");
                entity.Property(e => e.GenPhy)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("genPhy");
                entity.Property(e => e.GenSys)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("genSys");
                entity.Property(e => e.Hpc)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("HPC");
                entity.Property(e => e.Informt)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("informt");
                entity.Property(e => e.Injprescription)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("injprescription");
                entity.Property(e => e.IntraOralExam)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("intraOralExam");
                entity.Property(e => e.Investigate)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("investigate");
                entity.Property(e => e.IsAlarm).HasColumnName("isAlarm");
                entity.Property(e => e.IsDress)
                    .HasDefaultValue(false)
                    .HasColumnName("isDress");
                entity.Property(e => e.IsDrug)
                    .HasDefaultValue(false)
                    .HasColumnName("isDrug");
                entity.Property(e => e.IsInj)
                    .HasDefaultValue(false)
                    .HasColumnName("isInj");
                entity.Property(e => e.IsLab)
                    .HasDefaultValue(false)
                    .HasColumnName("isLab");
                entity.Property(e => e.IsLatest).HasColumnName("isLatest");
                entity.Property(e => e.IsReview).HasColumnName("isReview");
                entity.Property(e => e.IsServ)
                    .HasDefaultValue(false)
                    .HasColumnName("isServ");
                entity.Property(e => e.MedRpt).IsUnicode(false);
                entity.Property(e => e.NextApptDate)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("nextApptDate");
                entity.Property(e => e.PNo)
                    .HasMaxLength(50)
                    .HasColumnName("pNo");
                entity.Property(e => e.PhyExam)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("phyExam");
                entity.Property(e => e.Pmh)
                    .IsUnicode(false)
                    .HasColumnName("PMH");
                entity.Property(e => e.Preconsult)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
                entity.Property(e => e.Prescription)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("prescription");
                entity.Property(e => e.Referto)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("referto");
                entity.Property(e => e.Remarks)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("remarks");
                entity.Property(e => e.Services)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("services");
                entity.Property(e => e.Suppres)
                    .HasDefaultValue(false)
                    .HasColumnName("suppres");
                entity.Property(e => e.Symptoms)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("symptoms");
                entity.Property(e => e.SysReview)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("sysReview");
                entity.Property(e => e.TreatPlan)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("treatPlan");
                entity.Property(e => e.TreatType)
                    .HasMaxLength(3000)
                    .IsUnicode(false);
                entity.Property(e => e.Treatdone)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("treatdone");
                entity.Property(e => e.TreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("treatedBy");
                entity.Property(e => e.TreatplanBeforeEdit)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("treatplanBeforeEdit");
                entity.Property(e => e.TreatplanEdit)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("treatplanEdit");
                entity.Property(e => e.WardId)
                    .HasMaxLength(50)
                    .HasColumnName("WardID");
            });

            builder.Entity<HConsultingItem>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("hConsultingItems");

                entity.Property(e => e.AttendedTo).HasDefaultValue(false);
                entity.Property(e => e.Cdate)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("CDate");
                entity.Property(e => e.ConId).HasColumnName("ConID");
                entity.Property(e => e.ConsultId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName(consultIdColumn);
                entity.Property(e => e.Ctime)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("CTime");
                entity.Property(e => e.DrgName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DrgNAme");
                entity.Property(e => e.IsApprv)
                    .HasDefaultValue(false)
                    .HasColumnName("isApprv");
                entity.Property(e => e.Remarks)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Sno)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SNo");
            });

            builder.Entity<HDental>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("hDental");

                entity.Property(e => e.ClientCat)
                    .HasMaxLength(50)
                    .HasColumnName("clientCat");
                entity.Property(e => e.ConsultId)
                    .HasMaxLength(50)
                    .HasColumnName("consultID");
                entity.Property(e => e.DDate)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("dDate");
                entity.Property(e => e.DTime)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("dTime");
                entity.Property(e => e.IsDischarged).HasColumnName("isDischarged");
                entity.Property(e => e.PNo)
                    .HasMaxLength(50)
                    .HasColumnName("pNo");
                entity.Property(e => e.Reason).HasMaxLength(1000);
                entity.Property(e => e.Remarks).HasMaxLength(1000);
                entity.Property(e => e.Sno)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SNO");
            });

            builder.Entity<HDentalTreat>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("hDentalTreat");

                entity.Property(e => e.ARem)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("aRem");
                entity.Property(e => e.Allc).HasColumnName("ALLC");
                entity.Property(e => e.Alli1).HasColumnName("ALLI1");
                entity.Property(e => e.Alli2).HasColumnName("ALLI2");
                entity.Property(e => e.Allm1).HasColumnName("ALLM1");
                entity.Property(e => e.Allm2).HasColumnName("ALLM2");
                entity.Property(e => e.Allm3).HasColumnName("ALLM3");
                entity.Property(e => e.Allpm1).HasColumnName("ALLPM1");
                entity.Property(e => e.Allpm2).HasColumnName("ALLPM2");
                entity.Property(e => e.Alrc).HasColumnName("ALRC");
                entity.Property(e => e.Alri1).HasColumnName("ALRI1");
                entity.Property(e => e.Alri2).HasColumnName("ALRI2");
                entity.Property(e => e.Alrm1).HasColumnName("ALRM1");
                entity.Property(e => e.Alrm2).HasColumnName("ALRM2");
                entity.Property(e => e.Alrm3).HasColumnName("ALRM3");
                entity.Property(e => e.Alrpm1).HasColumnName("ALRPM1");
                entity.Property(e => e.Alrpm2).HasColumnName("ALRPM2");
                entity.Property(e => e.Aulc).HasColumnName("AULC");
                entity.Property(e => e.Auli1).HasColumnName("AULI1");
                entity.Property(e => e.Auli2).HasColumnName("AULI2");
                entity.Property(e => e.Aulm1).HasColumnName("AULM1");
                entity.Property(e => e.Aulm2).HasColumnName("AULM2");
                entity.Property(e => e.Aulm3).HasColumnName("AULM3");
                entity.Property(e => e.Aulpm1).HasColumnName("AULPM1");
                entity.Property(e => e.Aulpm2).HasColumnName("AULPM2");
                entity.Property(e => e.Aurc).HasColumnName("AURC");
                entity.Property(e => e.Auri1).HasColumnName("AURI1");
                entity.Property(e => e.Auri2).HasColumnName("AURI2");
                entity.Property(e => e.Aurm1).HasColumnName("AURM1");
                entity.Property(e => e.Aurm2).HasColumnName("AURM2");
                entity.Property(e => e.Aurm3).HasColumnName("AURM3");
                entity.Property(e => e.Aurpm1).HasColumnName("AURPM1");
                entity.Property(e => e.Aurpm2).HasColumnName("AURPM2");
                entity.Property(e => e.CRem)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("cRem");
                entity.Property(e => e.Cllc).HasColumnName("CLLC");
                entity.Property(e => e.Clli1).HasColumnName("CLLI1");
                entity.Property(e => e.Clli2).HasColumnName("CLLI2");
                entity.Property(e => e.Cllpm1).HasColumnName("CLLPM1");
                entity.Property(e => e.Cllpm2).HasColumnName("CLLPM2");
                entity.Property(e => e.Clrc).HasColumnName("CLRC");
                entity.Property(e => e.Clri1).HasColumnName("CLRI1");
                entity.Property(e => e.Clri2).HasColumnName("CLRI2");
                entity.Property(e => e.Clrpm1).HasColumnName("CLRPM1");
                entity.Property(e => e.Clrpm2).HasColumnName("CLRPM2");
                entity.Property(e => e.ConId)
                    .HasMaxLength(50)
                    .HasColumnName("conID");
                entity.Property(e => e.ConsultId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName(consultIdColumn);
                entity.Property(e => e.Culc).HasColumnName("CULC");
                entity.Property(e => e.Culi1).HasColumnName("CULI1");
                entity.Property(e => e.Culi2).HasColumnName("CULI2");
                entity.Property(e => e.Culpm1).HasColumnName("CULPM1");
                entity.Property(e => e.Culpm2).HasColumnName("CULPM2");
                entity.Property(e => e.Curc).HasColumnName("CURC");
                entity.Property(e => e.Curi1).HasColumnName("CURI1");
                entity.Property(e => e.Curi2).HasColumnName("CURI2");
                entity.Property(e => e.Curpm1).HasColumnName("CURPM1");
                entity.Property(e => e.Curpm2).HasColumnName("CURPM2");
                entity.Property(e => e.Dtype)
                    .HasMaxLength(1)
                    .HasColumnName("DType");
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(e => e.Pno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pno");
                entity.Property(e => e.TDate)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("tDate");
                entity.Property(e => e.TTime)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("tTime");
            });

            builder.Entity<ClinicType>(entity =>
            {
                entity.HasKey(e => e.ClinicId);

                entity.ToTable("ClinicTypes");

                entity.Property(e => e.ClinicId)
                    .HasMaxLength(50)
                    .HasColumnName("ClinicID");
                entity.Property(e => e.Apologies)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.ClinicDays)
                    .HasMaxLength(250)
                    .HasColumnName("clinicDays");
                entity.Property(e => e.ClinicName).HasMaxLength(100);
                entity.Property(e => e.ClinicPeriod)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.Designation)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.EmpId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EmpID");
                entity.Property(e => e.IdValCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("IDValCode");
                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PixName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.RctCode).HasMaxLength(2);
                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Sno)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SNO");
                entity.Property(e => e.Type).HasMaxLength(3);
            });

            builder.Entity<HClinicPurpose>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("hClinicPurpose");

                entity.Property(e => e.Purpose).HasMaxLength(50);
                entity.Property(e => e.Sno)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SNo");
            });

            builder.Entity<HPatient>(entity =>
            {
                entity.HasKey(e => e.Pno);

                entity.ToTable("hPatients");

                entity.HasIndex(e => new { e.PSurName, e.PFirstname }, "idx_FullName");

                entity.Property(e => e.Pno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PNo");
                entity.Property(e => e.Ancinfo)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasColumnName("ANCInfo");
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(appNameSql);
                entity.Property(e => e.Area)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.BioId)
                    .HasMaxLength(22)
                    .IsUnicode(false)
                    .HasComputedColumnSql("('PA'+CONVERT([varchar](20),[sno],(0)))", false)
                    .HasColumnName("BioID");
                entity.Property(e => e.BloodGroup)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Branch)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("branch");
                entity.Property(e => e.CardType)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ClientCatId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("clientCatID");
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql);
                entity.Property(e => e.Color)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Course)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.CoyClass)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.CoyName)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("coyNAme");
                entity.Property(e => e.CoyType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("coyType");
                entity.Property(e => e.Debt)
                    .HasDefaultValue(0m)
                    .HasColumnType(decimalType);
                entity.Property(e => e.DebtBf)
                    .HasDefaultValue(0m)
                    .HasColumnType(decimalType)
                    .HasColumnName("DebtBF");
                entity.Property(e => e.Dob)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("DOB");
                entity.Property(e => e.DrgRxn).HasMaxLength(4000);
                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .HasColumnName("email");
                entity.Property(e => e.EmpNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("empNo");
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql(getTimeSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.Expired).HasColumnName("expired");
                entity.Property(e => e.ExpiryDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.Faculty)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.FileDuration)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Genotype)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.HmoRef)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.HomeAddress)
                    .HasMaxLength(1100)
                    .IsUnicode(false);
                entity.Property(e => e.Introducedby)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("introducedby");
                entity.Property(e => e.IsEnrol)
                    .HasDefaultValue(false)
                    .HasColumnName("isEnrol");
                entity.Property(e => e.IsRev).HasColumnName("isRev");
                entity.Property(e => e.KinAddress)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("kinAddress");
                entity.Property(e => e.LasConDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.LastAttndDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.LastCheckDateForDebt)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.LastClinicVisited)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.LastConDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.LastConsultId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LastConsultID");
                entity.Property(e => e.LastDoctorSeen)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.LastPurpose)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.LatestBillNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Maturity)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Mstatus)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MStatus");
                entity.Property(e => e.NewReg)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("newReg");
                entity.Property(e => e.NextofKin)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Nokphone)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOKPhone");
                entity.Property(e => e.Occupation)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.OfficeAddress)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.OldPno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValue("NIL")
                    .HasColumnName("OldPNo");
                entity.Property(e => e.PCatId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("pCatID");
                entity.Property(e => e.PFirstname)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("pFirstname");
                entity.Property(e => e.PMembers)
                    .HasMaxLength(200)
                    .HasColumnName("pMembers");
                entity.Property(e => e.PPhoneNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("pPhoneNo");
                entity.Property(e => e.PSurName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("pSurName");
                entity.Property(e => e.PastMedHist).HasMaxLength(4000);
                entity.Property(e => e.PatPix).HasColumnType("image");
                entity.Property(e => e.PixName)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Pno2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PNo2");
                entity.Property(e => e.PolicyType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("policyType");
                entity.Property(e => e.Principal)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Ref)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.RegDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.RelationToKin)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("relationToKin");
                entity.Property(e => e.RelationToStaff)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("relationToStaff");
                entity.Property(e => e.Religion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Session)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Sex).HasMaxLength(50);
                entity.Property(e => e.SmsCat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("DOB_PAT")
                    .HasColumnName("smsCat");
                entity.Property(e => e.SmsNextDob)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("smsNextDOB");
                entity.Property(e => e.Sno)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SNo");
                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("status");
                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.TranStartDateForDebt)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            builder.Entity<HRecord>(entity =>
            {
                entity.HasKey(e => e.ConsultId)
                    .HasName("PK_hRecords_1")
                    ;

                entity.ToTable("hRecords", tb => tb.HasTrigger("trgRecDelete"));

                entity.HasIndex(e => e.RecDate, "idxRecDate");

                entity.Property(e => e.ConsultId)
                    .HasMaxLength(50)
                    .HasColumnName(consultIdColumn);
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(appNameSql);
                entity.Property(e => e.AttendedTo)
                    .HasDefaultValue(false)
                    .HasColumnName("attendedTo");
                entity.Property(e => e.AttendedToByDoc)
                    .HasDefaultValue(false)
                    .HasColumnName("attendedToByDoc");
                entity.Property(e => e.AttendedToByImmume).HasDefaultValue(false);
                entity.Property(e => e.AttendedToByNurse)
                    .HasDefaultValue(false)
                    .HasColumnName("attendedToByNurse");
                entity.Property(e => e.AttndStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("NORMAL");
                entity.Property(e => e.BillDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.ClientCat)
                    .HasMaxLength(50)
                    .HasColumnName("clientCat");
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql);
                entity.Property(e => e.ClinicType)
                    .HasMaxLength(50)
                    .HasColumnName("clinicType");
                entity.Property(e => e.ConsultIdnew)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ConsultIDNew");
                entity.Property(e => e.ConsultIdnew2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ConsultIDNew2");
                entity.Property(e => e.Coyname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Debt)
                    .HasDefaultValue(0m)
                    .HasColumnType(decimalType);
                entity.Property(e => e.Diagnosis)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
                entity.Property(e => e.DocAssigned).HasMaxLength(50);
                entity.Property(e => e.EmpId)
                    .HasMaxLength(50)
                    .HasColumnName("empID");
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))")
                    .HasColumnType("datetime");
                entity.Property(e => e.ExitDate).HasDefaultValueSql(getDateSql);
                entity.Property(e => e.ExitDateComment)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.HmoRef)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValue("NO");
                entity.Property(e => e.Htime)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("htime");
                entity.Property(e => e.IsJson)
                    .HasDefaultValue(false)
                    .HasColumnName("isJSon");
                entity.Property(e => e.LastAttndDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.LastClinicVisited)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.LastConsultId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LastConsultID");
                entity.Property(e => e.LastPurpose)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Mth)
                    .HasMaxLength(2)
                    .HasComputedColumnSql("(right('00'+CONVERT([nvarchar],datepart(month,isnull([billdate],[recdate])),(0)),(2)))", false);
                entity.Property(e => e.NextApptDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.PNo)
                    .HasMaxLength(50)
                    .HasColumnName("pNO");
                entity.Property(e => e.PatVal).HasDefaultValue((byte)0);
                entity.Property(e => e.RecDate)
                    .HasColumnType(dateTimeSql)
                    .HasColumnName("recDate");
                entity.Property(e => e.RecId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("recID");
                entity.Property(e => e.Referal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("referal");
                entity.Property(e => e.Remarks).HasMaxLength(100);
                entity.Property(e => e.Suppres)
                    .HasDefaultValue(false)
                    .HasColumnName("suppres");
                entity.Property(e => e.Tariff)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Yr)
                    .HasMaxLength(30)
                    .HasComputedColumnSql("(CONVERT([nvarchar],datepart(year,isnull([billdate],[recdate])),(0)))", false);
            });

            builder.Entity<HRetainership>(entity =>
            {
                entity.HasKey(e => e.RetainId);

                entity.ToTable("hRetainership");

                entity.Property(e => e.RetainId)
                    .HasMaxLength(50)
                    .HasColumnName("retainID");
                entity.Property(e => e.AcctId)
                    .HasMaxLength(50)
                    .HasColumnName("AcctID");
                entity.Property(e => e.Active)
                    .HasMaxLength(50)
                    .HasDefaultValue("YES");
                entity.Property(e => e.Address).HasMaxLength(50);
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(app_name())");
                entity.Property(e => e.CardRenewAmount)
                    .HasDefaultValue(0m)
                    .HasColumnType(decimalType);
                entity.Property(e => e.ClientCatId)
                    .HasMaxLength(50)
                    .HasColumnName("clientCatID");
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql);
                entity.Property(e => e.ClientType).HasMaxLength(50);
                entity.Property(e => e.ConAmount)
                    .HasDefaultValue(0m)
                    .HasColumnType(decimalType);
                entity.Property(e => e.Contact).HasMaxLength(50);
                entity.Property(e => e.DebtType).HasMaxLength(50);
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql(getTimeSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.Pcent).HasColumnName("PCent");
                entity.Property(e => e.PhoneNo).HasMaxLength(50);
                entity.Property(e => e.RegAmount)
                    .HasDefaultValue(0m)
                    .HasColumnType(decimalType);
                entity.Property(e => e.RetainCode).HasMaxLength(50);
                entity.Property(e => e.RetainDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("retainDate");
                entity.Property(e => e.RetainName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.UseTariff).HasMaxLength(50);
            });

            builder.Entity<BillAccum>(entity =>
            {
                entity.HasKey(e => new { e.consultID, e.SNo })
                    .HasName("PK__BillAccu__C81B94055DCC8612")
                    .HasFillFactor(80);

                entity.ToTable("BillAccum", tb => tb.HasTrigger("Prevent_Signed_Bill_Trigger"));

                entity.HasIndex(e => e.dtDate, "BillAccumDate").HasFillFactor(80);
                entity.HasIndex(e => e.consultID, "Consult_Idx").HasFillFactor(80);

                entity.Property(e => e.consultID).HasMaxLength(50);
                entity.Property(e => e.SNo).ValueGeneratedOnAdd();
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(appNameSql);
                entity.Property(e => e.AppVersion).HasDefaultValue(0);
                entity.Property(e => e.BillBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BillTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Capitated)
                    .HasMaxLength(3)
                    .HasDefaultValue("YES", "DF_BillAccum_Capitated");
                entity.Property(e => e.Category)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql);
                entity.Property(e => e.CoyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DRGCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Dept)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql(getTimeSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.Price).HasColumnType(decimalType);
                entity.Property(e => e.Qty).HasColumnType(decimalType);
                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.RevClinic)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Reversed).HasDefaultValue(false);
                entity.Property(e => e.ReversedPair).HasDefaultValue(0L);
                entity.Property(e => e.SubTotalSys)
                    .HasComputedColumnSql("(isnull([qty],(0))*isnull([price],(0)))", false)
                    .HasColumnType("decimal(37, 4)");
                entity.Property(e => e.TranID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Usage).HasMaxLength(500);
                entity.Property(e => e.attendedTo).HasDefaultValue(false, "DF_BillAccum_attendedTo");
                entity.Property(e => e.billtype)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.conID)
                    .HasMaxLength(50)
                    .HasComment("")
                    .HasDefaultValueSql("(NULL)", "DF_BillAccum_conID");
                entity.Property(e => e.drgName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.dtDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.isBilled).HasDefaultValue(false, "DF_BillAccum_isBilled");
                entity.Property(e => e.isOLD).HasDefaultValue(false);
                entity.Property(e => e.isRct).HasDefaultValue(false, "DF__BillAccum__isRct__503293D2");
                entity.Property(e => e.pNO).HasMaxLength(50);
                entity.Property(e => e.revType)
                    .HasMaxLength(50)
                    .HasDefaultValue("OTHERS", "DF__BillAccum__revTy__5872D418");
                entity.Property(e => e.subTotal).HasColumnType(decimalType);
                entity.Property(e => e.suppres).HasDefaultValue(false, "DF__billaccum__suppr__6E96540A");
                entity.Property(e => e.treatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            builder.Entity<Billing>(entity =>
            {
                entity.HasKey(e => e.billNO).HasFillFactor(80);

                entity.ToTable("Billing");

                entity.Property(e => e.billNO).HasMaxLength(50);
                entity.Property(e => e.AdmDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.AmountBilled).HasColumnType(decimalType);
                entity.Property(e => e.AmountBilledInWord).HasMaxLength(950);
                entity.Property(e => e.AmountPaid)
                    .HasDefaultValue(0m, "DF_Billing_AmountPaid")
                    .HasColumnType(decimalType);
                entity.Property(e => e.AmountSigned)
                    .HasDefaultValue(0m, "DF__Billing__AmountS__182E31D1")
                    .HasColumnType(decimalType);
                entity.Property(e => e.AmtBF).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(appNameSql, "DF__Billing__AppName__6C1AA569");
                entity.Property(e => e.ApprvCode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
                entity.Property(e => e.BillingMonth).HasMaxLength(50);
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql, "DF__Billing__ClientN__6B268130");
                entity.Property(e => e.DebtBF)
                    .HasDefaultValue(0m, "DF__Billing__DebtBF__2E527D1A")
                    .HasColumnType(decimalType);
                entity.Property(e => e.DischDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.Discount)
                    .HasDefaultValue(0m, "DF__Billing__Discoun__008BB26A")
                    .HasColumnType(decimalType);
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql, "DF__Billing__EntryDa__1DE70B27")
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql(getTimeSql, "DF__Billing__EntryTi__1BFEC2B5")
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.ID).ValueGeneratedOnAdd();
                entity.Property(e => e.InvNo).HasMaxLength(50);
                entity.Property(e => e.bDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.billType)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.clientID).HasMaxLength(150);
                entity.Property(e => e.consultDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.diagnosis)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
                entity.Property(e => e.isPaid).HasDefaultValue(false, "DF_Billing_isPaid");
                entity.Property(e => e.isPost).HasDefaultValue(false, "DF_Billing_isPost");
                entity.Property(e => e.isProcess).HasDefaultValue(false, "DF_Billing_isProcess");
                entity.Property(e => e.isSigned).HasDefaultValue(false, "DF__Billing__isSigne__173A0D98");
                entity.Property(e => e.pNo).HasMaxLength(50);
                entity.Property(e => e.profFee).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.timeVal)
                    .HasDefaultValueSql("(getdate())", "DF__Billing__timeVal__785593C5")
                    .HasColumnType(dateTimeSql);
            });

            builder.Entity<BillingDetail>(entity =>
            {
                entity.HasKey(e => new { e.billNO, e.SNO })
                    .HasName("PK__BillingD__B13C04EF60A8F2BD")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.billNO, "ConsultID_idx").HasFillFactor(80);

                entity.Property(e => e.billNO).HasMaxLength(50);
                entity.Property(e => e.AmtPaid)
                    .HasDefaultValue(0m, "DF_BillingDetails_AmtPaid")
                    .HasColumnType(decimalType);
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(appNameSql);
                entity.Property(e => e.AppVersion).HasDefaultValue(0, "DF_BillingDetails_AppVersion");
                entity.Property(e => e.BillBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BillHead)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.BillTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Capitated)
                    .HasMaxLength(3)
                    .HasDefaultValue("YES", "DF_BillingDetails_Capitated");
                entity.Property(e => e.Category)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql);
                entity.Property(e => e.CoyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DRGCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Dept)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Dosage).HasMaxLength(500);
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql(getTimeSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.RevClinic)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Reversed).HasDefaultValue(false);
                entity.Property(e => e.ReversedPair).HasDefaultValue(0L);
                entity.Property(e => e.TranID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.billType)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValue("SERVICE", "DF_BillingDetails_billType");
                entity.Property(e => e.conID).HasMaxLength(50);
                entity.Property(e => e.drgName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.dtDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.isOLD).HasDefaultValue(false, "DF__BillingDe__isOLD__359E9927");
                entity.Property(e => e.revType)
                    .HasMaxLength(50)
                    .HasDefaultValue("OTHERS", "DF__BillingDe__revTy__5966F851");
                entity.Property(e => e.subTotal).HasColumnType(decimalType);
                entity.Property(e => e.suppres).HasDefaultValue(false);
                entity.Property(e => e.treatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            builder.Entity<BillsForClient>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AmountBilled).HasColumnType(decimalType);
                entity.Property(e => e.AmountBilledInWord).HasMaxLength(950);
                entity.Property(e => e.AmountInvoiced).HasColumnType(decimalType);
                entity.Property(e => e.AmountPaid).HasColumnType(decimalType);
                entity.Property(e => e.AmtBF).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.AttendedToByClient).HasDefaultValue(false);
                entity.Property(e => e.BatchNo).HasMaxLength(50);
                entity.Property(e => e.BillMonth).HasMaxLength(50);
                entity.Property(e => e.BillYear).HasMaxLength(50);
                entity.Property(e => e.CoyCode).HasMaxLength(50);
                entity.Property(e => e.InvDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.InvNo).HasMaxLength(50);
                entity.Property(e => e.SNO).ValueGeneratedOnAdd();
                entity.Property(e => e.isOLd).HasDefaultValue(false, "DF_BillsForClients_isOLd");
                entity.Property(e => e.isPost).HasDefaultValue(false);
            });

            builder.Entity<DrugNHISListEntered>(entity =>
            {
                entity.HasKey(e => new { e.CoyID, e.Type }).HasFillFactor(80);

                entity.ToTable("DrugNHISListEntered");

                entity.Property(e => e.CoyID).HasMaxLength(50);
                entity.Property(e => e.Type).HasMaxLength(250);
                entity.Property(e => e.Remarks).HasMaxLength(50);
                entity.Property(e => e.UseTariff).HasMaxLength(50);
            });

            builder.Entity<Employee>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("Employees");

                entity.Property(e => e.AcadQual).HasMaxLength(250);
                entity.Property(e => e.BioID).HasMaxLength(52);
                entity.Property(e => e.DOB).HasColumnType(dateTimeSql);
                entity.Property(e => e.DeptID).HasMaxLength(50);
                entity.Property(e => e.Designation).HasMaxLength(50);
                entity.Property(e => e.DtExpectedConfirm).HasColumnType(dateTimeSql);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.EmpCat).HasMaxLength(50);
                entity.Property(e => e.EmpStatus).HasMaxLength(50);
                entity.Property(e => e.FirstGrtAddress).HasMaxLength(250);
                entity.Property(e => e.FirstGrtname).HasMaxLength(50);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.FirstRef).HasMaxLength(50);
                entity.Property(e => e.FirstRefAddress).HasMaxLength(250);
                entity.Property(e => e.HireDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.IDVal).HasMaxLength(50);
                entity.Property(e => e.JobDesc).HasMaxLength(3500);
                entity.Property(e => e.LGovt).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.NHSNo).HasMaxLength(50);
                entity.Property(e => e.NOK).HasMaxLength(250);
                entity.Property(e => e.NOKAddress).HasMaxLength(250);
                entity.Property(e => e.NOKPhone).HasMaxLength(50);
                entity.Property(e => e.NOKRel).HasMaxLength(50);
                entity.Property(e => e.NSITFNo).HasMaxLength(50);
                entity.Property(e => e.OtherName).HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.SNo).ValueGeneratedOnAdd();
                entity.Property(e => e.SalaryScale).HasMaxLength(100);
                entity.Property(e => e.SecondGrtAddress).HasMaxLength(250);
                entity.Property(e => e.SecondGrtName).HasMaxLength(50);
                entity.Property(e => e.SecondRef).HasMaxLength(50);
                entity.Property(e => e.SecondRefAddress).HasMaxLength(250);
                entity.Property(e => e.Sex).HasMaxLength(50);
                entity.Property(e => e.State).HasMaxLength(50);
                entity.Property(e => e.UnitID).HasMaxLength(50);
                entity.Property(e => e.empID).HasMaxLength(50);
                entity.Property(e => e.empPix).HasColumnType("image");
                entity.Property(e => e.homeAddress).HasMaxLength(250);
                entity.Property(e => e.maidenName).HasMaxLength(50);
                entity.Property(e => e.marital).HasMaxLength(50);
                entity.Property(e => e.spouseName).HasMaxLength(50);
            });

            builder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.ReceiptNo).HasFillFactor(80);

                entity.Property(e => e.ReceiptNo).HasMaxLength(50);
                entity.Property(e => e.AmountBilled).HasColumnType(decimalType);
                entity.Property(e => e.AmountInWord).HasMaxLength(550);
                entity.Property(e => e.AmountPaid).HasColumnType(decimalType);
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(appNameSql);
                entity.Property(e => e.BankCode).HasMaxLength(50);
                entity.Property(e => e.ChequeDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.ChequeNo).HasMaxLength(50);
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql);
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql(getTimeSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.ReceiptDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.Receivedby).HasMaxLength(50);
                entity.Property(e => e.Remarks)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.RetainCode).HasMaxLength(50);
                entity.Property(e => e.SNo).ValueGeneratedOnAdd();
                entity.Property(e => e.ValueDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.billNO).HasMaxLength(50);
                entity.Property(e => e.clinicID).HasMaxLength(50);
                entity.Property(e => e.isPost).HasDefaultValue(false, "DF_Payments_isPost");
                entity.Property(e => e.pNO).HasMaxLength(50);
                entity.Property(e => e.payType).HasMaxLength(50);
                entity.Property(e => e.paymentFor).HasMaxLength(1000);
                entity.Property(e => e.rTime).HasColumnType(dateTimeSql);
                entity.Property(e => e.suppres).HasDefaultValue(false);
            });

            builder.Entity<PaymentDetail>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AmountPaid).HasColumnType(decimalType);
                entity.Property(e => e.AmountToPay).HasColumnType(decimalType);
                entity.Property(e => e.BillDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.BillItem)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.ReceiptDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.ReceiptNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RevType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SNo).ValueGeneratedOnAdd();
                entity.Property(e => e.billNO).HasMaxLength(50);
                entity.Property(e => e.suppres).HasDefaultValue(false);
            });

            builder.Entity<PaymentType>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.AmountPaid).HasColumnType(decimalType);
                entity.Property(e => e.AppName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(appNameSql);
                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql(hostNameSql);
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql(getDateSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.EntryTime)
                    .HasDefaultValueSql(getTimeSql)
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.PayType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ReceiptDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.ReceiptNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SNo).ValueGeneratedOnAdd();
                entity.Property(e => e.TranID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.isPost).HasDefaultValue(false, "DF__PaymentTy__isPos__14DCC3DA");
                entity.Property(e => e.reversed).HasDefaultValue(false);
                entity.Property(e => e.suppres).HasDefaultValue(false);
            });

            builder.Entity<PaymentTypeDetail>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AmountPaid).HasColumnType(decimalType);
                entity.Property(e => e.AmountToPay).HasColumnType(decimalType);
                entity.Property(e => e.BillDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.BillItem)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.PayType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ReceiptDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.ReceiptNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RevType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SNo).ValueGeneratedOnAdd();
                entity.Property(e => e.billNO).HasMaxLength(50);
            });

            builder.Entity<hAppointment>(entity =>
            {
                entity
                    .HasKey(e => e.ID);

                entity.ToTable("hAppointment");

                entity.HasIndex(e => e.ApptDate, "idx_ApptDate").HasFillFactor(80);

                entity.Property(e => e.ApptDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.ApptTime).HasColumnType(dateTimeSql);
                entity.Property(e => e.EmpID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ID).ValueGeneratedOnAdd();
                entity.Property(e => e.clinicType)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.entryDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.entryTime).HasColumnType(dateTimeSql);
                entity.Property(e => e.pno)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.remarks)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            builder.Entity<hReferal>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("hReferal");

                entity.Property(e => e.AttendedTo).HasDefaultValue(false, "DF_hReferal_AttendedTo");
                entity.Property(e => e.AttendedToByRec).HasDefaultValue(false);
                entity.Property(e => e.Comments).HasMaxLength(500);
                entity.Property(e => e.EmpID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ID).ValueGeneratedOnAdd();
                entity.Property(e => e.Remarks)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.apptDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.apptTime).HasColumnType(dateTimeSql);
                entity.Property(e => e.clientCat).HasMaxLength(50);
                entity.Property(e => e.conID).HasMaxLength(50);
                entity.Property(e => e.consultID).HasMaxLength(50);
                entity.Property(e => e.pNo).HasMaxLength(50);
                entity.Property(e => e.refAddress).HasMaxLength(2000);
                entity.Property(e => e.refDate).HasColumnType(dateTimeSql);
                entity.Property(e => e.refReason).HasMaxLength(1000);
                entity.Property(e => e.refTime).HasColumnType(dateTimeSql);
                entity.Property(e => e.referTo).HasMaxLength(500);
                entity.Property(e => e.suppres).HasDefaultValue(false, "DF__hreferal__suppre__7266E4EE");
            });

            builder.Entity<hRevenueType>(entity =>
            {
                entity.HasKey(e => e.RevType).HasFillFactor(80);

                entity.ToTable("hRevenueType");

                entity.Property(e => e.RevType).HasMaxLength(50);
                entity.Property(e => e.AccountNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Active)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.CatRemarks).HasMaxLength(50);
                entity.Property(e => e.PatNameDumm).HasMaxLength(150);
                entity.Property(e => e.RevDateDumm)
                    .HasDefaultValueSql("(getdate())", "DF_hRevenueType_RevDateDumm")
                    .HasColumnType(dateTimeSql);
                entity.Property(e => e.SNO).ValueGeneratedOnAdd();
                entity.Property(e => e.Serial).HasDefaultValue(99, "DF_hRevenueType_Serial");
                entity.Property(e => e.Subtotal).HasDefaultValue(0.0, "DF_hRevenueType_Subtotal");
            });

            builder.Entity<hService>(entity =>
            {
                entity.HasKey(e => e.Service)
                    .HasName("PK_hServices2")
                    .HasFillFactor(80);

                entity.Property(e => e.Service)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Capitated).HasMaxLength(3);
                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.SNo).ValueGeneratedOnAdd();
                entity.Property(e => e.ServiceID)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.TYPE).HasMaxLength(50);
                entity.Property(e => e._3MTHLY).HasColumnName("3MTHLY");
                entity.Property(e => e._6MTHLY).HasColumnName("6MTHLY");
                entity.Property(e => e.revType).HasMaxLength(50);
            });

            builder.Entity<hServiceNHI>(entity =>
            {
                entity.HasKey(e => e.SNO)
                    .HasName("hServiceNHIS_pk")
                    .HasFillFactor(80);

                entity.ToTable("hServiceNHIS");

                entity.Property(e => e.Capitated)
                    .HasMaxLength(50)
                    .HasDefaultValue("NO", "DF__hServiceN__Capit__3B818FAF");
                entity.Property(e => e.Category)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Company).HasMaxLength(255);
                entity.Property(e => e.CoyName).HasMaxLength(250);
                entity.Property(e => e.Remarks).HasMaxLength(255);
                entity.Property(e => e.RevType)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Service).HasMaxLength(255);
                entity.Property(e => e.TariffStatus).HasMaxLength(50);
                entity.Property(e => e.UsersCat)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            builder.Entity<vwEmpCashier>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwEmpCashier");

                entity.Property(e => e.EmpName).HasMaxLength(101);
                entity.Property(e => e.empID).HasMaxLength(50);
            });

            builder.Entity<vwEmpName>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwEmpName");

                entity.Property(e => e.Dept).HasMaxLength(50);
                entity.Property(e => e.Designation).HasMaxLength(100);
                entity.Property(e => e.EmpName).HasMaxLength(101);
                entity.Property(e => e.empID).HasMaxLength(50);
            });

            builder.Entity<vwEmployee>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwEmployees");

                entity.Property(e => e.Authorizer).HasMaxLength(50);
                entity.Property(e => e.DeptName).HasMaxLength(50);
                entity.Property(e => e.Designation).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.EmpCat).HasMaxLength(50);
                entity.Property(e => e.EmpNo).HasMaxLength(50);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Sex).HasMaxLength(50);
                entity.Property(e => e.Surname).HasMaxLength(50);
                entity.Property(e => e.homeAddress).HasMaxLength(250);
                entity.Property(e => e.statName).HasMaxLength(50);
            });

            builder.Entity<AestheticPatient>(entity =>
            {
                entity.ToTable("AestheticPatients");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(30).IsUnicode(false);
                entity.Property(e => e.SkinType).HasMaxLength(50);
                entity.Property(e => e.MedicalHistory).HasMaxLength(4000).IsUnicode(false);

                entity.HasMany(e => e.Consultations)
                      .WithOne(c => c.Patient)
                      .HasForeignKey(c => c.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AestheticConsultation>(entity =>
            {
                entity.ToTable("AestheticConsultations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProcedureType).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.TreatmentPlan).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.ProcedureDescription).HasMaxLength(4000).IsUnicode(false);
                entity.Property(e => e.ConsentGiven).HasDefaultValue(false);
                entity.Property(e => e.ConsultationDate).HasColumnType(dateTimeSql);

                entity.HasMany(e => e.Photos)
                      .WithOne(p => p.Consultation)
                      .HasForeignKey(p => p.ConsultationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AestheticPhoto>(entity =>
            {
                entity.ToTable("AestheticPhotos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FilePath).HasMaxLength(4000).IsUnicode(false);
                entity.Property(e => e.FileName).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.Type).HasMaxLength(100).IsUnicode(false);
            });
        }

        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddAuditInfo();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddAuditInfo()
        {
            var currentUserId = _userIdAccessor.GetCurrentUserId();

            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity &&
                           (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = currentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = currentUserId;
            }
        }
    }
}
