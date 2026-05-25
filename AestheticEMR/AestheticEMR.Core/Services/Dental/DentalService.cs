using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Dental;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Dental.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Services.Dental;

public class DentalService(ApplicationDbContext dbContext) : IDentalService
{
    // --- Odontogram (HDentalTreat) ---

    public IEnumerable<HDentalTreat> GetCharts() =>
        dbContext.HDentalTreats.OrderByDescending(t => t.TDate).ToList();

    public HDentalTreat? GetChartById(long id) =>
        dbContext.HDentalTreats.FirstOrDefault(t => t.Id == id);

    public IEnumerable<HDentalTreat> GetChartsByPno(string pno) =>
        dbContext.HDentalTreats.Where(t => t.Pno == pno).OrderByDescending(t => t.TDate).ToList();

    public HDentalTreat AddChart(HDentalTreat chart)
    {
        EnsureChartDateTimes(chart);
        dbContext.HDentalTreats.Add(chart);
        dbContext.SaveChanges();
        return chart;
    }

    public HDentalTreat UpdateChart(HDentalTreat chart, string currentUserId)
    {
        EnsureChartDateTimes(chart);

        var existing = dbContext.HDentalTreats.Find(chart.Id)
            ?? throw new KeyNotFoundException($"Dental chart not found: {chart.Id}");

        if (!string.Equals(existing.ConId, currentUserId, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Only the author that created this clinical record can update it.");

        ApplyChartValues(existing, chart);

        dbContext.SaveChanges();
        return existing;
    }

    public void DeleteChart(long id, string currentUserId)
    {
        var entity = dbContext.HDentalTreats.Find(id)
            ?? throw new KeyNotFoundException($"Dental chart not found: {id}");

        if (!string.Equals(entity.ConId, currentUserId, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Only the author that created this clinical record can delete it.");

        dbContext.HDentalTreats.Remove(entity);
        dbContext.SaveChanges();
    }

    // --- Imaging ---

    public IEnumerable<DentalImaging> GetImagingRecords() =>
        dbContext.DentalImagings.OrderByDescending(i => i.ImagingDate).ToList();

    public DentalImaging? GetImagingById(int id) =>
        dbContext.DentalImagings.Find(id);

    public IEnumerable<DentalImaging> GetImagingByPno(string pno) =>
        dbContext.DentalImagings.Where(i => i.Pno == pno).OrderByDescending(i => i.ImagingDate).ToList();

    public DentalImaging AddImaging(DentalImaging imaging)
    {
        var now = DateTime.UtcNow;
        imaging.ImagingDate = NormalizeSqlDateTime(imaging.ImagingDate, now);
        imaging.CreatedDate = now;
        imaging.UpdatedDate = now;
        dbContext.DentalImagings.Add(imaging);
        dbContext.SaveChanges();
        return imaging;
    }

    public DentalImaging UpdateImaging(DentalImaging imaging, string currentUserId)
    {
        var existing = dbContext.DentalImagings.Find(imaging.Id)
            ?? throw new KeyNotFoundException($"Dental imaging not found: {imaging.Id}");

        if (!string.Equals(existing.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Only the author that created this clinical record can update it.");

        imaging.ImagingDate = NormalizeSqlDateTime(imaging.ImagingDate, DateTime.UtcNow);
        ApplyImagingValues(existing, imaging);
        existing.UpdatedDate = DateTime.UtcNow;

        dbContext.SaveChanges();
        return existing;
    }

    public void DeleteImaging(int id, string currentUserId)
    {
        var entity = dbContext.DentalImagings.Find(id)
            ?? throw new KeyNotFoundException($"Dental imaging not found: {id}");

        if (!string.Equals(entity.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Only the author that created this clinical record can delete it.");

        dbContext.DentalImagings.Remove(entity);
        dbContext.SaveChanges();
    }

    // --- Combined Encounter ---

    public (HDentalTreat Chart, DentalImaging Imaging, HConsulting Consulting) SaveEncounter(
        HDentalTreat chart,
        DentalImaging imaging,
        HConsulting consulting,
        string currentUserId)
    {
        using var tx = dbContext.Database.BeginTransaction();

        var now = DateTime.UtcNow;

        chart.Pno = chart.Pno?.Trim() ?? string.Empty;
        chart.ConsultId = chart.ConsultId?.Trim() ?? string.Empty;
        imaging.Pno = imaging.Pno?.Trim() ?? chart.Pno;
        imaging.ConsultId = imaging.ConsultId?.Trim() ?? chart.ConsultId;

        EnsureChartDateTimes(chart);
        imaging.ImagingDate = NormalizeSqlDateTime(imaging.ImagingDate, now);

        if (string.IsNullOrWhiteSpace(chart.Pno) || string.IsNullOrWhiteSpace(chart.ConsultId))
            throw new InvalidOperationException("PNO and ConsultId are required.");

        HDentalTreat persistedChart;
        if (chart.Id > 0)
        {
            persistedChart = dbContext.HDentalTreats.Find(chart.Id)
                ?? throw new KeyNotFoundException($"Dental chart not found: {chart.Id}");

            if (!string.Equals(persistedChart.ConId, currentUserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only the author that created this clinical record can update it.");

            ApplyChartValues(persistedChart, chart);
        }
        else
        {
            chart.ConId = string.IsNullOrWhiteSpace(chart.ConId) ? currentUserId : chart.ConId;
            dbContext.HDentalTreats.Add(chart);
            persistedChart = chart;
        }

        DentalImaging persistedImaging;
        if (imaging.Id > 0)
        {
            persistedImaging = dbContext.DentalImagings.Find(imaging.Id)
                ?? throw new KeyNotFoundException($"Dental imaging not found: {imaging.Id}");

            if (!string.Equals(persistedImaging.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only the author that created this clinical record can update it.");

            ApplyImagingValues(persistedImaging, imaging);
            persistedImaging.UpdatedDate = now;
        }
        else
        {
            var existingImaging = dbContext.DentalImagings
                .FirstOrDefault(x => x.ConsultId == chart.ConsultId && x.Pno == chart.Pno);

            if (existingImaging != null)
            {
                if (!string.Equals(existingImaging.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
                    throw new UnauthorizedAccessException("Only the author that created this clinical record can update it.");

                persistedImaging = existingImaging;
                ApplyImagingValues(persistedImaging, imaging);
                persistedImaging.UpdatedDate = now;
            }
            else
            {
                imaging.CreatedBy = currentUserId;
                imaging.CreatedDate = now;
                imaging.UpdatedDate = now;
                dbContext.DentalImagings.Add(imaging);
                persistedImaging = imaging;
            }
        }

        var existingConsulting = dbContext.HConsultings
            .FirstOrDefault(x => x.ConsultId == chart.ConsultId && x.PNo == chart.Pno);

        HConsulting persistedConsulting;
        if (existingConsulting != null)
        {
            persistedConsulting = existingConsulting;
            persistedConsulting.Diagnosis = consulting.Diagnosis;
            persistedConsulting.Complaints = consulting.Complaints;
            persistedConsulting.Hpc = consulting.Hpc;
            persistedConsulting.Pmh = consulting.Pmh;
            persistedConsulting.DentHist = consulting.DentHist;
            persistedConsulting.DrugHx = consulting.DrugHx;
            persistedConsulting.Prescription = consulting.Prescription;
            persistedConsulting.Services = consulting.Services;
            persistedConsulting.Investigate = consulting.Investigate;
            persistedConsulting.TreatPlan = consulting.TreatPlan;
            persistedConsulting.ClientCat = string.IsNullOrWhiteSpace(consulting.ClientCat) ? persistedConsulting.ClientCat : consulting.ClientCat;
            persistedConsulting.EditDate = now;
            persistedConsulting.EditTime = now;
            persistedConsulting.TreatedBy = string.IsNullOrWhiteSpace(currentUserId) ? persistedConsulting.TreatedBy : currentUserId;
        }
        else
        {
            consulting.ConsultId = chart.ConsultId;
            consulting.PNo = chart.Pno;
            consulting.ClientCat = string.IsNullOrWhiteSpace(consulting.ClientCat) ? "PRIVATE" : consulting.ClientCat;
            consulting.CDate = now;
            consulting.CTime = now;
            consulting.TreatedBy = string.IsNullOrWhiteSpace(currentUserId) ? "SYSTEM" : currentUserId;
            consulting.Diagnosis = consulting.Diagnosis;
            consulting.TreatPlan = consulting.TreatPlan;

            dbContext.HConsultings.Add(consulting);
            persistedConsulting = consulting;
        }

        dbContext.SaveChanges();
        tx.Commit();

        return (persistedChart, persistedImaging, persistedConsulting);
    }

    public (HDentalTreat Chart, DentalImaging Imaging, HConsulting Consulting)? GetEncounter(string consultId, string pno)
    {
        var chart = dbContext.HDentalTreats
            .FirstOrDefault(x => x.ConsultId == consultId && x.Pno == pno);
        if (chart == null) return null;

        var imaging = dbContext.DentalImagings
            .OrderByDescending(x => x.UpdatedDate)
            .FirstOrDefault(x => x.ConsultId == consultId && x.Pno == pno)
            ?? new DentalImaging
            {
                Pno = pno,
                ConsultId = consultId,
                ImagingDate = DateTime.UtcNow
            };

        var consulting = dbContext.HConsultings
            .OrderByDescending(x => x.EditDate ?? x.CDate)
            .FirstOrDefault(x => x.ConsultId == consultId && x.PNo == pno)
            ?? new HConsulting
            {
                ConsultId = consultId,
                PNo = pno,
                ClientCat = "PRIVATE",
                TreatedBy = "SYSTEM",
                CDate = DateTime.UtcNow
            };

        return (chart, imaging, consulting);
    }

    private static string? BuildTreatPlan(string? prescription, string? services, string? investigate)
    {
        var parts = new[]
        {
            string.IsNullOrWhiteSpace(prescription) ? null : $"Prescription: {prescription.Trim()}",
            string.IsNullOrWhiteSpace(services) ? null : $"Services: {services.Trim()}",
            string.IsNullOrWhiteSpace(investigate) ? null : $"Investigate: {investigate.Trim()}"
        }
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .ToArray();

        return parts.Length == 0 ? null : string.Join("\n", parts);
    }

    private static void ApplyChartValues(HDentalTreat existing, HDentalTreat chart)
    {
        existing.Pno = chart.Pno;
        existing.ConsultId = chart.ConsultId;
        existing.Dtype = chart.Dtype;
        existing.TDate = NormalizeSqlDateTime(chart.TDate, DateTime.UtcNow);
        existing.TTime = NormalizeSqlDateTime(chart.TTime, existing.TDate);
        existing.TeethStatusJson = chart.TeethStatusJson;
        existing.OrthodonticsJson = chart.OrthodonticsJson;
        existing.OralExamJson = chart.OralExamJson;
        existing.ARem = chart.ARem;
        existing.CRem = chart.CRem;
        existing.ConId = chart.ConId;

        existing.Auli1 = chart.Auli1; existing.Auli2 = chart.Auli2; existing.Aulc = chart.Aulc;
        existing.Aulpm1 = chart.Aulpm1; existing.Aulpm2 = chart.Aulpm2;
        existing.Aulm1 = chart.Aulm1; existing.Aulm2 = chart.Aulm2; existing.Aulm3 = chart.Aulm3;
        existing.Auri1 = chart.Auri1; existing.Auri2 = chart.Auri2; existing.Aurc = chart.Aurc;
        existing.Aurpm1 = chart.Aurpm1; existing.Aurpm2 = chart.Aurpm2;
        existing.Aurm1 = chart.Aurm1; existing.Aurm2 = chart.Aurm2; existing.Aurm3 = chart.Aurm3;
        existing.Alli1 = chart.Alli1; existing.Alli2 = chart.Alli2; existing.Allc = chart.Allc;
        existing.Allpm1 = chart.Allpm1; existing.Allpm2 = chart.Allpm2;
        existing.Allm1 = chart.Allm1; existing.Allm2 = chart.Allm2; existing.Allm3 = chart.Allm3;
        existing.Alri1 = chart.Alri1; existing.Alri2 = chart.Alri2; existing.Alrc = chart.Alrc;
        existing.Alrpm1 = chart.Alrpm1; existing.Alrpm2 = chart.Alrpm2;
        existing.Alrm1 = chart.Alrm1; existing.Alrm2 = chart.Alrm2; existing.Alrm3 = chart.Alrm3;
        existing.Culi1 = chart.Culi1; existing.Culi2 = chart.Culi2; existing.Culc = chart.Culc;
        existing.Culpm1 = chart.Culpm1; existing.Culpm2 = chart.Culpm2;
        existing.Curi1 = chart.Curi1; existing.Curi2 = chart.Curi2; existing.Curc = chart.Curc;
        existing.Curpm1 = chart.Curpm1; existing.Curpm2 = chart.Curpm2;
        existing.Clli1 = chart.Clli1; existing.Clli2 = chart.Clli2; existing.Cllc = chart.Cllc;
        existing.Cllpm1 = chart.Cllpm1; existing.Cllpm2 = chart.Cllpm2;
        existing.Clri1 = chart.Clri1; existing.Clri2 = chart.Clri2; existing.Clrc = chart.Clrc;
        existing.Clrpm1 = chart.Clrpm1; existing.Clrpm2 = chart.Clrpm2;
    }

    private static void EnsureChartDateTimes(HDentalTreat chart)
    {
        var fallback = DateTime.UtcNow;
        chart.TDate = NormalizeSqlDateTime(chart.TDate, fallback);
        chart.TTime = NormalizeSqlDateTime(chart.TTime, chart.TDate);
    }

    private static DateTime NormalizeSqlDateTime(DateTime value, DateTime fallback)
    {
        var min = new DateTime(1753, 1, 1);
        var max = new DateTime(9999, 12, 31, 23, 59, 59, 997);

        if (value < min || value > max)
            return fallback < min || fallback > max ? min : fallback;

        return value;
    }

    private static void ApplyImagingValues(DentalImaging existing, DentalImaging imaging)
    {
        existing.Pno = imaging.Pno;
        existing.ConsultId = imaging.ConsultId;
        existing.ImagingDate = imaging.ImagingDate;
        existing.ImagingType = imaging.ImagingType;
        existing.ToothRegion = imaging.ToothRegion;
        existing.Findings = imaging.Findings;
        existing.Impression = imaging.Impression;
        existing.Recommendations = imaging.Recommendations;
        existing.FilePath = imaging.FilePath;
        existing.FileName = imaging.FileName;
        existing.Notes = imaging.Notes;
    }
}
