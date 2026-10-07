using Gochs.ProtectiveStructures.DTOs.ProtectiveStructures;
using Gochs.ProtectiveStructures.Entities;
using Gochs.ProtectiveStructures.Exceptions;
using Gochs.ProtectiveStructures.Mappings;
using Gochs.ProtectiveStructures.Repositories.Interfaces;
using Gochs.ProtectiveStructures.Services.Interfaces;

namespace Gochs.ProtectiveStructures.Services.Implementations;

public class ProtectiveStructureService : IProtectiveStructureService
{
    private readonly IProtectiveStructureRepository protectiveStructureRepository;

    public ProtectiveStructureService(IProtectiveStructureRepository protectiveStructureRepository)
    {
        this.protectiveStructureRepository = protectiveStructureRepository;
    }

    public async Task<IReadOnlyCollection<ProtectiveStructureDto>> GetAllAsync()
    {
        var entities = await protectiveStructureRepository.GetAllAsync();
        var result = new List<ProtectiveStructureDto>(entities.Count);

        foreach (var entity in entities)
        {
            var assignedCount = await protectiveStructureRepository.GetAssignedEmployeeCountAsync(entity.Id);
            result.Add(entity.ToDto(assignedCount));
        }

        return result;
    }

    public async Task<ProtectiveStructureDetailsDto> GetByIdAsync(int id)
    {
        var entity = await protectiveStructureRepository.GetDetailsAsync(id)
            ?? throw new NotFoundException($"Защитное сооружение с id {id} не найдено.");

        var assignedCount = entity.Employees.Count;

        return new ProtectiveStructureDetailsDto
        {
            Id = entity.Id,
            RegistrationNumber = entity.RegistrationNumber,
            Name = entity.Name,
            Type = entity.Type,
            Address = entity.Address,
            Capacity = entity.Capacity,
            AssignedEmployeeCount = assignedCount,
            AvailableCapacity = entity.Capacity - assignedCount,
            Condition = entity.Condition,
            ResponsiblePerson = entity.ResponsiblePerson,
            Phone = entity.Phone,
            Notes = entity.Notes,
            CreatedAt = entity.CreatedAt,
            Employees = entity.Employees.OrderBy(x => x.FullName).Select(x => x.ToDto()).ToList(),
            Inspections = entity.Inspections.OrderByDescending(x => x.InspectionDate).Select(x => x.ToDto()).ToList()
        };
    }

    public async Task<ProtectiveStructureDto> CreateAsync(CreateProtectiveStructureDto dto)
    {
        var registrationNumber = RequireText(dto.RegistrationNumber, "Регистрационный номер");
        var name = RequireText(dto.Name, "Название");
        var address = RequireText(dto.Address, "Адрес");

        if (await protectiveStructureRepository.RegistrationNumberExistsAsync(registrationNumber))
            throw new BusinessRuleException($"Регистрационный номер '{registrationNumber}' уже используется.");

        var entity = new ProtectiveStructure
        {
            RegistrationNumber = registrationNumber,
            Name = name,
            Type = dto.Type!.Value,
            Address = address,
            Capacity = dto.Capacity,
            ResponsiblePerson = Normalize(dto.ResponsiblePerson),
            Phone = Normalize(dto.Phone),
            Notes = Normalize(dto.Notes)
        };

        await protectiveStructureRepository.AddAsync(entity);
        return entity.ToDto(0);
    }

    public async Task<ProtectiveStructureDto> UpdateAsync(int id, UpdateProtectiveStructureDto dto)
    {
        var entity = await protectiveStructureRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Защитное сооружение с id {id} не найдено.");

        var assignedCount = await protectiveStructureRepository.GetAssignedEmployeeCountAsync(id);
        if (dto.Capacity < assignedCount)
            throw new BusinessRuleException(
                $"Вместимость нельзя уменьшить до {dto.Capacity}: уже распределено сотрудников — {assignedCount}.");

        var registrationNumber = RequireText(dto.RegistrationNumber, "Регистрационный номер");
        if (await protectiveStructureRepository.RegistrationNumberExistsAsync(registrationNumber, id))
            throw new BusinessRuleException($"Регистрационный номер '{registrationNumber}' уже используется.");

        entity.RegistrationNumber = registrationNumber;
        entity.Name = RequireText(dto.Name, "Название");
        entity.Type = dto.Type!.Value;
        entity.Address = RequireText(dto.Address, "Адрес");
        entity.Capacity = dto.Capacity;
        entity.Condition = dto.Condition!.Value;
        entity.ResponsiblePerson = Normalize(dto.ResponsiblePerson);
        entity.Phone = Normalize(dto.Phone);
        entity.Notes = Normalize(dto.Notes);

        await protectiveStructureRepository.UpdateAsync(entity);
        return entity.ToDto(assignedCount);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await protectiveStructureRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Защитное сооружение с id {id} не найдено.");

        if (await protectiveStructureRepository.GetAssignedEmployeeCountAsync(id) > 0)
            throw new BusinessRuleException("Нельзя удалить сооружение, пока к нему распределены сотрудники.");

        if (await protectiveStructureRepository.HasInspectionsAsync(id))
            throw new BusinessRuleException("Нельзя удалить сооружение, пока у него есть история проверок.");

        await protectiveStructureRepository.DeleteAsync(entity);
    }

    private static string RequireText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} не может быть пустым.");

        return value.Trim();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
