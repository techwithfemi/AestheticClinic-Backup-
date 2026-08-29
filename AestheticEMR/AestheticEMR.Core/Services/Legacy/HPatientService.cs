using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Services.Legacy;

public class HPatientService(ApplicationDbContext context) : IHPatientService
{
    public async Task<IEnumerable<HPatient>> GetAllAsync()
    {
        return await context.Vwhpatients
            .AsNoTracking()
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Firstname)
            .Select(x => new HPatient
            {
                Pno = x.Pno,
                OldPno = x.OldPno,
                PSurName = x.Surname,
                PFirstname = x.Firstname,
                Title = x.Title,
                Sex = x.Sex,
                Mstatus = x.Mstatus,
                Dob = x.Dob,
                Occupation = x.Occupation,
                HomeAddress = x.HomeAddress,
                OfficeAddress = x.OfficeAddress,
                PPhoneNo = x.PhoneNo,
                Email = x.Email,
                EmpNo = x.EmpNo,
                Branch = x.Branch,
                NextofKin = x.NextofKin,
                KinAddress = x.KinAddress,
                RelationToKin = x.RelationToKin,
                PCatId = x.PatCat,
                CoyType = x.CoyType,
                CoyName = x.CoyName,
                ClientName = x.Client,
                ClientCatId = x.BillingCat,
                PolicyType = x.PolicyType,
                Nokphone = x.Nokphone,
                Status = x.Status,
                RegDate = x.RegDate,
                LastAttndDate = x.LastAttndDate,
                LastClinicVisited = x.LastClinicVisited,
                LastPurpose = x.Purpose,
                LastConsultId = x.LastConsultId,
                LatestBillNo = x.LatestBillNo,
                LastDoctorSeen = x.LastDoctorSeen,
                LastConDate = x.LastConDate,
                Debt = x.Debt,
                DebtBf = x.Debt,
                UserName = x.UserName,
                EntryDate = x.EntryDate,
                NewReg = x.NewReg,
                CardType = x.CardType,
                ExpiryDate = x.ExpiryDate,
                Expired = x.Expired,
                HmoRef = x.HmoRef,
                CoyClass = x.CoyClass,
                Principal = x.Principal,
                PastMedHist = x.PastMedHist,
                Area = x.Area,
                Maturity = x.Maturity,
                AdmissionDaysLimit = x.AdmissionDaysLimit,
                CumNoOfAdmissionDaysPerAnnum = x.CumNoOfAdmissionDaysPerAnnum
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<HPatient>> GetByRegDateAsync(DateTime regDate)
    {
        return await context.Vwhpatients
            .AsNoTracking()
            .Where(x => x.RegDate.HasValue && x.RegDate.Value.Date == regDate.Date)
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Firstname)
            .Select(x => new HPatient
            {
                Pno = x.Pno,
                OldPno = x.OldPno,
                PSurName = x.Surname,
                PFirstname = x.Firstname,
                Title = x.Title,
                Sex = x.Sex,
                Mstatus = x.Mstatus,
                Dob = x.Dob,
                Occupation = x.Occupation,
                HomeAddress = x.HomeAddress,
                OfficeAddress = x.OfficeAddress,
                PPhoneNo = x.PhoneNo,
                Email = x.Email,
                EmpNo = x.EmpNo,
                Branch = x.Branch,
                NextofKin = x.NextofKin,
                KinAddress = x.KinAddress,
                RelationToKin = x.RelationToKin,
                PCatId = x.PatCat,
                CoyType = x.CoyType,
                CoyName = x.CoyName,
                ClientName = x.Client,
                ClientCatId = x.BillingCat,
                PolicyType = x.PolicyType,
                Nokphone = x.Nokphone,
                Status = x.Status,
                RegDate = x.RegDate,
                LastAttndDate = x.LastAttndDate,
                LastClinicVisited = x.LastClinicVisited,
                LastPurpose = x.Purpose,
                LastConsultId = x.LastConsultId,
                LatestBillNo = x.LatestBillNo,
                LastDoctorSeen = x.LastDoctorSeen,
                LastConDate = x.LastConDate,
                Debt = x.Debt,
                DebtBf = x.Debt,
                UserName = x.UserName,
                EntryDate = x.EntryDate,
                NewReg = x.NewReg,
                CardType = x.CardType,
                ExpiryDate = x.ExpiryDate,
                Expired = x.Expired,
                HmoRef = x.HmoRef,
                CoyClass = x.CoyClass,
                Principal = x.Principal,
                PastMedHist = x.PastMedHist,
                Area = x.Area,
                Maturity = x.Maturity,
                AdmissionDaysLimit = x.AdmissionDaysLimit,
                CumNoOfAdmissionDaysPerAnnum = x.CumNoOfAdmissionDaysPerAnnum
            })
            .ToListAsync();
    }

    public async Task<HPatient?> GetByIdAsync(string pno)
    {
        return await context.HPatients.FirstOrDefaultAsync(x => x.Pno == pno);
    }

    public async Task<HPatient> CreateAsync(HPatient patient)
    {
        patient.Pno = await GeneratePnoAsync();
        patient.CardType = string.IsNullOrWhiteSpace(patient.CardType) ? "single" : patient.CardType;
        context.HPatients.Add(patient);
        await context.SaveChangesAsync();
        return patient;
    }

    public async Task<HPatient> UpdateAsync(HPatient patient)
    {
        patient.CardType = string.IsNullOrWhiteSpace(patient.CardType) ? "single" : patient.CardType;

        // If no new photo was supplied, preserve the existing one
        if (patient.PatPix == null || patient.PatPix.Length == 0)
        {
            var existing = await context.HPatients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Pno == patient.Pno);

            if (existing?.PatPix != null && existing.PatPix.Length > 0)
            {
                patient.PatPix = existing.PatPix;
            }
        }

        var entry = context.Entry(patient);
        if (entry.State == EntityState.Detached)
        {
            context.HPatients.Attach(patient);
            entry = context.Entry(patient);
        }

        entry.State = EntityState.Modified;
        entry.Property(x => x.Sno).IsModified = false;

        await context.SaveChangesAsync();
        return patient;
    }

    public async Task DeleteAsync(string pno)
    {
        var patient = await GetByIdAsync(pno);
        if (patient is null) return;

        var hasAttendance = await context.HRecords.AnyAsync(r => r.PNo == pno);
        if (hasAttendance)
        {
            throw new InvalidOperationException("This patient is referenced by attendance records and cannot be deleted.");
        }

        context.HPatients.Remove(patient);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Generates the next patient number by calling the legacy SQL Server
    /// stored procedure <c>getIDNo</c> — converted from the VB6 <c>getIDNo</c> Sub.
    ///
    /// VB6 equivalent:
    /// <code>
    /// cmd.CommandText = "getIDNo"
    /// cmd.Parameters.Append cmd.CreateParameter("DestName", adVarChar, adParamInput, 50, destName)
    /// getID_No = rs!getIDNo &amp; ""
    /// </code>
    /// </summary>
    private async Task<string> GeneratePnoAsync()
    {
        // EF Core 7+ SqlQuery<T> executes the stored proc and maps the
        // single-column result set to a list of strings.
        var results = await context.Database
            .SqlQuery<string>($"EXEC getIDNo @DestName = {"PATIENT2"}")
            .ToListAsync();

        var generatedId = results.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(generatedId))
        {
            throw new InvalidOperationException(
                "Stored procedure 'getIDNo' returned no value for destination 'PATIENT2'.");
        }

        return generatedId;
    }
}
