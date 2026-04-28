using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Dental;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Dental.Interfaces;

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
        dbContext.HDentalTreats.Add(chart);
        dbContext.SaveChanges();
        return chart;
    }

    public HDentalTreat UpdateChart(HDentalTreat chart)
    {
        var existing = dbContext.HDentalTreats.Find(chart.Id)
            ?? throw new KeyNotFoundException($"Dental chart not found: {chart.Id}");

        existing.Pno = chart.Pno;
        existing.ConsultId = chart.ConsultId;
        existing.Dtype = chart.Dtype;
        existing.TDate = chart.TDate;
        existing.TTime = chart.TTime;
        existing.ARem = chart.ARem;
        existing.CRem = chart.CRem;
        existing.ConId = chart.ConId;

        // tooth flags — Adult Upper Left
        existing.Auli1 = chart.Auli1; existing.Auli2 = chart.Auli2; existing.Aulc = chart.Aulc;
        existing.Aulpm1 = chart.Aulpm1; existing.Aulpm2 = chart.Aulpm2;
        existing.Aulm1 = chart.Aulm1; existing.Aulm2 = chart.Aulm2; existing.Aulm3 = chart.Aulm3;
        // Adult Upper Right
        existing.Auri1 = chart.Auri1; existing.Auri2 = chart.Auri2; existing.Aurc = chart.Aurc;
        existing.Aurpm1 = chart.Aurpm1; existing.Aurpm2 = chart.Aurpm2;
        existing.Aurm1 = chart.Aurm1; existing.Aurm2 = chart.Aurm2; existing.Aurm3 = chart.Aurm3;
        // Adult Lower Left
        existing.Alli1 = chart.Alli1; existing.Alli2 = chart.Alli2; existing.Allc = chart.Allc;
        existing.Allpm1 = chart.Allpm1; existing.Allpm2 = chart.Allpm2;
        existing.Allm1 = chart.Allm1; existing.Allm2 = chart.Allm2; existing.Allm3 = chart.Allm3;
        // Adult Lower Right
        existing.Alri1 = chart.Alri1; existing.Alri2 = chart.Alri2; existing.Alrc = chart.Alrc;
        existing.Alrpm1 = chart.Alrpm1; existing.Alrpm2 = chart.Alrpm2;
        existing.Alrm1 = chart.Alrm1; existing.Alrm2 = chart.Alrm2; existing.Alrm3 = chart.Alrm3;
        // Child Upper Left / Right
        existing.Culi1 = chart.Culi1; existing.Culi2 = chart.Culi2; existing.Culc = chart.Culc;
        existing.Culpm1 = chart.Culpm1; existing.Culpm2 = chart.Culpm2;
        existing.Curi1 = chart.Curi1; existing.Curi2 = chart.Curi2; existing.Curc = chart.Curc;
        existing.Curpm1 = chart.Curpm1; existing.Curpm2 = chart.Curpm2;
        // Child Lower Left / Right
        existing.Clli1 = chart.Clli1; existing.Clli2 = chart.Clli2; existing.Cllc = chart.Cllc;
        existing.Cllpm1 = chart.Cllpm1; existing.Cllpm2 = chart.Cllpm2;
        existing.Clri1 = chart.Clri1; existing.Clri2 = chart.Clri2; existing.Clrc = chart.Clrc;
        existing.Clrpm1 = chart.Clrpm1; existing.Clrpm2 = chart.Clrpm2;

        dbContext.SaveChanges();
        return existing;
    }

    public void DeleteChart(long id)
    {
        var entity = dbContext.HDentalTreats.Find(id)
            ?? throw new KeyNotFoundException($"Dental chart not found: {id}");
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
        imaging.CreatedDate = DateTime.UtcNow;
        imaging.UpdatedDate = DateTime.UtcNow;
        dbContext.DentalImagings.Add(imaging);
        dbContext.SaveChanges();
        return imaging;
    }

    public DentalImaging UpdateImaging(DentalImaging imaging)
    {
        var existing = dbContext.DentalImagings.Find(imaging.Id)
            ?? throw new KeyNotFoundException($"Dental imaging not found: {imaging.Id}");

        existing.ImagingDate = imaging.ImagingDate;
        existing.ImagingType = imaging.ImagingType;
        existing.ToothRegion = imaging.ToothRegion;
        existing.Findings = imaging.Findings;
        existing.Impression = imaging.Impression;
        existing.Recommendations = imaging.Recommendations;
        existing.FilePath = imaging.FilePath;
        existing.FileName = imaging.FileName;
        existing.Notes = imaging.Notes;
        existing.UpdatedDate = DateTime.UtcNow;

        dbContext.SaveChanges();
        return existing;
    }

    public void DeleteImaging(int id)
    {
        var entity = dbContext.DentalImagings.Find(id)
            ?? throw new KeyNotFoundException($"Dental imaging not found: {id}");
        dbContext.DentalImagings.Remove(entity);
        dbContext.SaveChanges();
    }
}
