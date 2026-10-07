using Gochs.ProtectiveStructures.DTOs.Employees;

namespace Gochs.ProtectiveStructures.Services.Interfaces;

public interface IEmployeeService
{
    Task<IReadOnlyCollection<EmployeeDto>> GetAllAsync();
    Task<IReadOnlyCollection<EmployeeDto>> GetByProtectiveStructureIdAsync(int protectiveStructureId);
    Task<EmployeeDto> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task<EmployeeDto> UpdateAssignmentAsync(int id, UpdateEmployeeAssignmentDto dto);
    Task DeleteAsync(int id);
}
