using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Models.Dental;

namespace AestheticEMR.Core.Services.Dental.Interfaces;

public interface IDentalService
{
    // --- Odontogram (HDentalTreat) ---
    IEnumerable<HDentalTreat> GetCharts();
    HDentalTreat? GetChartById(long id);
    IEnumerable<HDentalTreat> GetChartsByPno(string pno);
    HDentalTreat AddChart(HDentalTreat chart);
    HDentalTreat UpdateChart(HDentalTreat chart);
    void DeleteChart(long id);

    // --- Imaging ---
    IEnumerable<DentalImaging> GetImagingRecords();
    DentalImaging? GetImagingById(int id);
    IEnumerable<DentalImaging> GetImagingByPno(string pno);
    DentalImaging AddImaging(DentalImaging imaging);
    DentalImaging UpdateImaging(DentalImaging imaging);
    void DeleteImaging(int id);

    // --- Combined Encounter (single transaction: HDentalTreat + DentalImaging + HConsulting) ---
    (HDentalTreat Chart, DentalImaging Imaging, HConsulting Consulting) SaveEncounter(
        HDentalTreat chart,
        DentalImaging imaging,
        HConsulting consulting,
        string currentUserId);

    (HDentalTreat Chart, DentalImaging Imaging, HConsulting Consulting)? GetEncounter(string consultId, string pno);
}
