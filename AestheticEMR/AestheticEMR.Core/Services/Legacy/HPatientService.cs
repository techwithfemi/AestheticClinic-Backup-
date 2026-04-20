using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Services.Legacy;

public class HPatientService(ApplicationDbContext context) : IHPatientService
{
    public async Task<IEnumerable<HPatient>> GetAllAsync()
    {
        return await context.HPatients
            .OrderBy(x => x.PSurName)
            .ThenBy(x => x.PFirstname)
            .ToListAsync();
    }

    public async Task<HPatient?> GetByIdAsync(string pno)
    {
        return await context.HPatients.FirstOrDefaultAsync(x => x.Pno == pno);
    }

    public async Task<HPatient> CreateAsync(HPatient patient)
    {
        patient.Pno = await GeneratePnoAsync();
        context.HPatients.Add(patient);
        await context.SaveChangesAsync();
        return patient;
    }

    public async Task<HPatient> UpdateAsync(HPatient patient)
    {
        context.HPatients.Update(patient);
        await context.SaveChangesAsync();
        return patient;
    }

    public async Task DeleteAsync(string pno)
    {
        var patient = await GetByIdAsync(pno);
        if (patient is null) return;

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
