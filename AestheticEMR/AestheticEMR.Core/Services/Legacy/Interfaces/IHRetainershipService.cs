using AestheticEMR.Core.Models.Legacy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IHRetainershipService
{
    Task<IEnumerable<HRetainership>> GetAllAsync();
    Task<HRetainership?> GetByIdAsync(string retainId);
    Task<HRetainership> CreateAsync(HRetainership retainership);
    Task<HRetainership> UpdateAsync(HRetainership retainership);
    Task DeleteAsync(string retainId);
}