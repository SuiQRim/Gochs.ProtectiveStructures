using Gochs.ProtectiveStructures.Entities;

namespace Gochs.ProtectiveStructures.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<List<Employee>> GetByProtectiveStructureIdAsync(int protectiveStructureId);
    Task<Employee?> GetByIdAsync(int id);
    Task<bool> PersonnelNumberExistsAsync(string personnelNumber, int? excludeId = null);
    Task AddAsync(Employee entity);
    Task UpdateAsync(Employee entity);
    Task DeleteAsync(Employee entity);
}
