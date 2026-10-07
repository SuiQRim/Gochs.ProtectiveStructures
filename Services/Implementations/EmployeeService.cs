using Gochs.ProtectiveStructures.DTOs.Employees;
using Gochs.ProtectiveStructures.Entities;
using Gochs.ProtectiveStructures.Enums;
using Gochs.ProtectiveStructures.Exceptions;
using Gochs.ProtectiveStructures.Mappings;
using Gochs.ProtectiveStructures.Repositories.Interfaces;
using Gochs.ProtectiveStructures.Services.Interfaces;

namespace Gochs.ProtectiveStructures.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository employeeRepository;
    private readonly IProtectiveStructureRepository protectiveStructureRepository;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IProtectiveStructureRepository protectiveStructureRepository)
    {
        this.employeeRepository = employeeRepository;
        this.protectiveStructureRepository = protectiveStructureRepository;
    }

    public async Task<IReadOnlyCollection<EmployeeDto>> GetAllAsync() =>
        (await employeeRepository.GetAllAsync()).Select(x => x.ToDto()).ToList();

    public async Task<IReadOnlyCollection<EmployeeDto>> GetByProtectiveStructureIdAsync(int protectiveStructureId)
    {
        await EnsureProtectiveStructureExistsAsync(protectiveStructureId);
        return (await employeeRepository.GetByProtectiveStructureIdAsync(protectiveStructureId))
            .Select(x => x.ToDto())
            .ToList();
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        var entity = await employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Сотрудник с id {id} не найден.");

        return entity.ToDto();
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var personnelNumber = RequireText(dto.PersonnelNumber, "Табельный номер");

        if (await employeeRepository.PersonnelNumberExistsAsync(personnelNumber))
            throw new BusinessRuleException($"Табельный номер '{personnelNumber}' уже используется.");

        var entity = new Employee
        {
            PersonnelNumber = personnelNumber,
            FullName = RequireText(dto.FullName, "ФИО"),
            Department = RequireText(dto.Department, "Подразделение"),
            Position = RequireText(dto.Position, "Должность")
        };

        await employeeRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var entity = await employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Сотрудник с id {id} не найден.");

        var personnelNumber = RequireText(dto.PersonnelNumber, "Табельный номер");

        if (await employeeRepository.PersonnelNumberExistsAsync(personnelNumber, id))
            throw new BusinessRuleException($"Табельный номер '{personnelNumber}' уже используется.");

        entity.PersonnelNumber = personnelNumber;
        entity.FullName = RequireText(dto.FullName, "ФИО");
        entity.Department = RequireText(dto.Department, "Подразделение");
        entity.Position = RequireText(dto.Position, "Должность");

        await employeeRepository.UpdateAsync(entity);
        return entity.ToDto();
    }

    public async Task<EmployeeDto> UpdateAssignmentAsync(int id, UpdateEmployeeAssignmentDto dto)
    {
        var employee = await employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Сотрудник с id {id} не найден.");

        if (!dto.ProtectiveStructureId.HasValue)
        {
            employee.ProtectiveStructureId = null;
            await employeeRepository.UpdateAsync(employee);
            return employee.ToDto();
        }

        var structureId = dto.ProtectiveStructureId.Value;

        if (employee.ProtectiveStructureId == structureId)
            return employee.ToDto();

        var structure = await protectiveStructureRepository.GetByIdAsync(structureId)
            ?? throw new NotFoundException($"Защитное сооружение с id {structureId} не найдено.");

        if (structure.Condition == ProtectiveStructureCondition.NotReady)
            throw new BusinessRuleException("Нельзя распределить сотрудника в неготовое защитное сооружение.");

        var assignedCount = await protectiveStructureRepository.GetAssignedEmployeeCountAsync(structureId);
        if (assignedCount >= structure.Capacity)
            throw new BusinessRuleException("В защитном сооружении нет свободных мест.");

        employee.ProtectiveStructureId = structureId;
        await employeeRepository.UpdateAsync(employee);

        return employee.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Сотрудник с id {id} не найден.");

        await employeeRepository.DeleteAsync(entity);
    }

    private async Task EnsureProtectiveStructureExistsAsync(int id)
    {
        if (!await protectiveStructureRepository.ExistsAsync(id))
            throw new NotFoundException($"Защитное сооружение с id {id} не найдено.");
    }

    private static string RequireText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} не может быть пустым.");

        return value.Trim();
    }
}
