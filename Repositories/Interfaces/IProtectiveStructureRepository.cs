using Gochs.ProtectiveStructures.Entities;

namespace Gochs.ProtectiveStructures.Repositories.Interfaces;

public interface IProtectiveStructureRepository
{
    Task<List<ProtectiveStructure>> GetAllAsync();
    Task<ProtectiveStructure?> GetByIdAsync(int id);
    Task<ProtectiveStructure?> GetDetailsAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> RegistrationNumberExistsAsync(string registrationNumber, int? excludeId = null);
    Task<int> GetAssignedEmployeeCountAsync(int id);
    Task<bool> HasInspectionsAsync(int id);
    Task AddAsync(ProtectiveStructure entity);
    Task UpdateAsync(ProtectiveStructure entity);
    Task DeleteAsync(ProtectiveStructure entity);
}
