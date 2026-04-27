using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AestheticEMR.Core.Services.Legacy;

public class HRetainershipService : IHRetainershipService
{
    private readonly ApplicationDbContext _context;

    public HRetainershipService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HRetainership>> GetAllAsync()
    {
        return await _context.HRetainerships
            .OrderBy(r => r.RetainName)
            .ToListAsync();
    }

    public async Task<HRetainership?> GetByIdAsync(string retainId)
    {
        return await _context.HRetainerships
            .FirstOrDefaultAsync(r => r.RetainId == retainId);
    }

    public async Task<HRetainership> CreateAsync(HRetainership retainership)
    {
        // Generate RetainId and RetainCode
        var generatedCode = await GenerateRetainCodeAsync();
        retainership.RetainId = generatedCode;
        retainership.RetainCode = generatedCode;

        _context.HRetainerships.Add(retainership);
        await _context.SaveChangesAsync();
        return retainership;
    }

    public async Task<HRetainership> UpdateAsync(HRetainership retainership)
    {
        _context.HRetainerships.Update(retainership);
        await _context.SaveChangesAsync();
        return retainership;
    }

    public async Task DeleteAsync(string retainId)
    {
        var retainership = await GetByIdAsync(retainId);
        if (retainership != null)
        {
            var isReferencedByPatients = await _context.HPatients.AnyAsync(p => p.CoyName == retainId);
            if (isReferencedByPatients)
            {
                throw new InvalidOperationException("This company is referenced by patients and cannot be deleted.");
            }

            _context.HRetainerships.Remove(retainership);
            await _context.SaveChangesAsync();
        }
    }

    private async Task<string> GenerateRetainCodeAsync()
    {
        // Get the maximum right 4 characters of retaincode from the table
        var maxCode = await _context.HRetainerships
            .Where(r => !string.IsNullOrEmpty(r.RetainCode) && r.RetainCode.Length >= 4)
            .Select(r => r.RetainCode.Substring(r.RetainCode.Length - 4))
            .ToListAsync();

        int maxId = 0;
        foreach (var code in maxCode)
        {
            if (int.TryParse(code, out int id))
            {
                maxId = Math.Max(maxId, id);
            }
        }

        // Increment the ID
        int nextId = maxId + 1;

        // For now, we'll use a simple format. You may need to adjust based on Coy_Code logic
        // The VB6 code checks for Coy_Code = "LONG" and prepends strHospID
        // Since we don't have access to these variables, we'll use a simple 4-digit format
        string generatedCode = nextId.ToString("D4");

        return generatedCode;
    }
}