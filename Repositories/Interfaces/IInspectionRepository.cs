using Gochs.ProtectiveStructures.Entities;

namespace Gochs.ProtectiveStructures.Repositories.Interfaces;

public interface IInspectionRepository
{
    Task<List<Inspection>> GetAllAsync();
    Task<List<Inspection>> GetByProtectiveStructureIdAsync(int protectiveStructureId);
    Task<Inspection?> GetByIdAsync(int id);
    Task AddAsync(Inspection entity);
}
