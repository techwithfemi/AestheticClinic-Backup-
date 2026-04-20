using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Models.Legacy;

public partial class TempScaffoldContext : DbContext
{
    public TempScaffoldContext()
    {
    }

    public TempScaffoldContext(DbContextOptions<TempScaffoldContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HConsulting> HConsultings { get; set; }

    public virtual DbSet<HConsultingItem> HConsultingItems { get; set; }

    public virtual DbSet<HDental> HDentals { get; set; }

    public virtual DbSet<HDentalTreat> HDentalTreats { get; set; }

    public virtual DbSet<HPatient> HPatients { get; set; }

    public virtual DbSet<HRecord> HRecords { get; set; }

    public virtual DbSet<HRetainership> HRetainerships { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LOGIC;Database=Hospital_Aesthetic_EMR;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<HConsulting>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.ToTable("hConsulting");

            entity.HasIndex(e => e.CDate, "IX_hConsulting").HasFillFactor(80);

            entity.HasIndex(e => e.CDate, "idxCdate").HasFillFactor(80);

            entity.HasIndex(e => e.PNo, "idxPno").HasFillFactor(80);

            entity.HasIndex(e => e.CDate, "idx_CDate").HasFillFactor(80);

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())");
            entity.Property(e => e.AttendedTo)
                .HasDefaultValue(false, "DF_hConsulting_attendedTo")
                .HasColumnName("attendedTo");
            entity.Property(e => e.AttendedToByHmo)
                .HasDefaultValue(false)
                .HasColumnName("AttendedToByHMO");
            entity.Property(e => e.AttendedToByPharm)
                .HasDefaultValue(false, "DF_hConsulting_attendedToByPharm")
                .HasColumnName("attendedToByPharm");
            entity.Property(e => e.AttendedtoByLab).HasDefaultValue(false, "DF_hConsulting_AttendedtoByLab_1");
            entity.Property(e => e.BillRemarks)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.CDate)
                .HasColumnType("datetime")
                .HasColumnName("cDate");
            entity.Property(e => e.CTime)
                .HasColumnType("datetime")
                .HasColumnName("cTime");
            entity.Property(e => e.ClientCat)
                .HasMaxLength(50)
                .HasColumnName("clientCat");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())");
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
                .HasColumnName("consultID");
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
            entity.Property(e => e.EditDate).HasColumnType("datetime");
            entity.Property(e => e.EditTime).HasColumnType("datetime");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF__hConsulti__Entry__180E3640")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_hConsulting_EntryTime")
                .HasColumnType("datetime");
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
                .HasDefaultValue(false, "DF_hConsulting_isDress")
                .HasColumnName("isDress");
            entity.Property(e => e.IsDrug)
                .HasDefaultValue(false, "DF_hConsulting_isDrug_1")
                .HasColumnName("isDrug");
            entity.Property(e => e.IsInj)
                .HasDefaultValue(false, "DF_hConsulting_isInj")
                .HasColumnName("isInj");
            entity.Property(e => e.IsLab)
                .HasDefaultValue(false, "DF_hConsulting_isLab_1")
                .HasColumnName("isLab");
            entity.Property(e => e.IsLatest).HasColumnName("isLatest");
            entity.Property(e => e.IsReview).HasColumnName("isReview");
            entity.Property(e => e.IsServ)
                .HasDefaultValue(false, "DF_hConsulting_isServ_1")
                .HasColumnName("isServ");
            entity.Property(e => e.MedRpt).IsUnicode(false);
            entity.Property(e => e.NextApptDate)
                .HasColumnType("datetime")
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
                .HasDefaultValue(false, "DF__hConsulti__suppr__6DA22FD1")
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

            entity.Property(e => e.AttendedTo).HasDefaultValue(false, "DF_hConsultingItems_AttendedTo");
            entity.Property(e => e.Cdate)
                .HasColumnType("datetime")
                .HasColumnName("CDate");
            entity.Property(e => e.ConId).HasColumnName("ConID");
            entity.Property(e => e.ConsultId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ConsultID");
            entity.Property(e => e.Ctime)
                .HasColumnType("datetime")
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
                .HasColumnType("datetime")
                .HasColumnName("dDate");
            entity.Property(e => e.DTime)
                .HasColumnType("datetime")
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
                .HasColumnName("consultID");
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
                .HasColumnType("datetime")
                .HasColumnName("tDate");
            entity.Property(e => e.TTime)
                .HasColumnType("datetime")
                .HasColumnName("tTime");
        });

        builder.Entity<HPatient>(entity =>
        {
            entity.HasKey(e => e.Pno).HasFillFactor(80);

            entity.ToTable("hPatients");

            entity.HasIndex(e => new { e.PSurName, e.PFirstname }, "idx_FullName").HasFillFactor(80);

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
                .HasDefaultValueSql("(app_name())", "DF__hPatients__AppNa__49C58D65");
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
                .HasDefaultValueSql("(host_name())", "DF__hPatients__Clien__48D1692C");
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
                .HasDefaultValue(0m, "DF_ALLPAT$_Debt")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DebtBf)
                .HasDefaultValue(0m, "DF_ALLPAT$_DebtBF")
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("DebtBF");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
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
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_ALLPAT$_EntryDate")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF__hPatients__Entry__47DD44F3")
                .HasColumnType("datetime");
            entity.Property(e => e.Expired).HasColumnName("expired");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
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
                .HasDefaultValue(false, "DF_ALLPAT$_isEnrol")
                .HasColumnName("isEnrol");
            entity.Property(e => e.IsRev).HasColumnName("isRev");
            entity.Property(e => e.KinAddress)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("kinAddress");
            entity.Property(e => e.LasConDate).HasColumnType("datetime");
            entity.Property(e => e.LastAttndDate).HasColumnType("datetime");
            entity.Property(e => e.LastCheckDateForDebt)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF__hPatients__LastC__43D7A9E5")
                .HasColumnType("datetime");
            entity.Property(e => e.LastClinicVisited)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LastConDate).HasColumnType("datetime");
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
                .HasDefaultValue("NIL", "DF_ALLPAT$_OldPNo")
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
            entity.Property(e => e.RegDate).HasColumnType("datetime");
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
                .HasDefaultValue("DOB_PAT", "DF__hPatients__smsCa__151CBAFC")
                .HasColumnName("smsCat");
            entity.Property(e => e.SmsNextDob)
                .HasColumnType("datetime")
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
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF__hPatients__TranS__42E385AC")
                .HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        builder.Entity<HRecord>(entity =>
        {
            entity.HasKey(e => e.ConsultId)
                .HasName("PK_hRecords_1")
                .HasFillFactor(80);

            entity.ToTable("hRecords", tb => tb.HasTrigger("trgRecDelete"));

            entity.HasIndex(e => e.RecDate, "idxRecDate").HasFillFactor(80);

            entity.Property(e => e.ConsultId)
                .HasMaxLength(50)
                .HasColumnName("consultID");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())");
            entity.Property(e => e.AttendedTo)
                .HasDefaultValue(false, "DF_hRecords_attendedTo")
                .HasColumnName("attendedTo");
            entity.Property(e => e.AttendedToByDoc)
                .HasDefaultValue(false, "DF_hRecords_attendedToByDoc")
                .HasColumnName("attendedToByDoc");
            entity.Property(e => e.AttendedToByImmume).HasDefaultValue(false, "DF__hRecords__Attend__6C84B9BD");
            entity.Property(e => e.AttendedToByNurse)
                .HasDefaultValue(false, "DF_hRecords_attendedToByDoc1")
                .HasColumnName("attendedToByNurse");
            entity.Property(e => e.AttndStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("NORMAL", "DF__hRecords__AttndS__09360704");
            entity.Property(e => e.BillDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))")
                .HasColumnType("datetime");
            entity.Property(e => e.ClientCat)
                .HasMaxLength(50)
                .HasColumnName("clientCat");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())");
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
                .HasDefaultValue(0m, "DF__hRecords__Debt__0F78F03F")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DocAssigned).HasMaxLength(50);
            entity.Property(e => e.EmpId)
                .HasMaxLength(50)
                .HasColumnName("empID");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))")
                .HasColumnType("datetime");
            entity.Property(e => e.ExitDate).HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_hRecords_ExitDate");
            entity.Property(e => e.ExitDateComment)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HmoRef)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("NO");
            entity.Property(e => e.Htime)
                .HasColumnType("datetime")
                .HasColumnName("htime");
            entity.Property(e => e.IsJson)
                .HasDefaultValue(false, "DF__hRecords__isJSon__72F1C02A")
                .HasColumnName("isJSon");
            entity.Property(e => e.LastAttndDate).HasColumnType("datetime");
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
            entity.Property(e => e.NextApptDate).HasColumnType("datetime");
            entity.Property(e => e.PNo)
                .HasMaxLength(50)
                .HasColumnName("pNO");
            entity.Property(e => e.PatVal).HasDefaultValue((byte)0, "DF_hRecords_PatVal");
            entity.Property(e => e.RecDate)
                .HasColumnType("datetime")
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
                .HasDefaultValue(false, "DF__hrecords__suppre__6BB9E75F")
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
            entity.HasKey(e => e.RetainId).HasFillFactor(80);

            entity.ToTable("hRetainership");

            entity.Property(e => e.RetainId)
                .HasMaxLength(50)
                .HasColumnName("retainID");
            entity.Property(e => e.AcctId)
                .HasMaxLength(50)
                .HasColumnName("AcctID");
            entity.Property(e => e.Active)
                .HasMaxLength(50)
                .HasDefaultValue("YES", "DF_hRetainership3_Active");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())");
            entity.Property(e => e.CardRenewAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClientCatId)
                .HasMaxLength(50)
                .HasColumnName("clientCatID");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())");
            entity.Property(e => e.ClientType).HasMaxLength(50);
            entity.Property(e => e.ConAmount)
                .HasDefaultValue(0m, "DF_hRetainership_ConAmount")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.DebtType).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))")
                .HasColumnType("datetime");
            entity.Property(e => e.Pcent).HasColumnName("PCent");
            entity.Property(e => e.PhoneNo).HasMaxLength(50);
            entity.Property(e => e.RegAmount)
                .HasDefaultValue(0m, "DF_hRetainership_RegAmount")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RetainCode).HasMaxLength(50);
            entity.Property(e => e.RetainDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("retainDate");
            entity.Property(e => e.RetainName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UseTariff).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
