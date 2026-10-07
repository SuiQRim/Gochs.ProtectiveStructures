using Gochs.ProtectiveStructures.DTOs.Employees;
using Gochs.ProtectiveStructures.DTOs.Inspections;
using Gochs.ProtectiveStructures.DTOs.ProtectiveStructures;
using Gochs.ProtectiveStructures.Entities;

namespace Gochs.ProtectiveStructures.Mappings;

public static class DtoMappings
{
    public static EmployeeDto ToDto(this Employee entity) => new()
    {
        Id = entity.Id,
        PersonnelNumber = entity.PersonnelNumber,
        FullName = entity.FullName,
        Department = entity.Department,
        Position = entity.Position,
        ProtectiveStructureId = entity.ProtectiveStructureId,
        CreatedAt = entity.CreatedAt
    };

    public static InspectionDto ToDto(this Inspection entity) => new()
    {
        Id = entity.Id,
        ProtectiveStructureId = entity.ProtectiveStructureId,
        InspectionDate = entity.InspectionDate,
        InspectorName = entity.InspectorName,
        ResultCondition = entity.ResultCondition,
        Findings = entity.Findings,
        RequiredActions = entity.RequiredActions,
        NextInspectionDate = entity.NextInspectionDate,
        CreatedAt = entity.CreatedAt
    };

    public static ProtectiveStructureDto ToDto(this ProtectiveStructure entity, int assignedEmployeeCount) => new()
    {
        Id = entity.Id,
        RegistrationNumber = entity.RegistrationNumber,
        Name = entity.Name,
        Type = entity.Type,
        Address = entity.Address,
        Capacity = entity.Capacity,
        AssignedEmployeeCount = assignedEmployeeCount,
        AvailableCapacity = entity.Capacity - assignedEmployeeCount,
        Condition = entity.Condition,
        ResponsiblePerson = entity.ResponsiblePerson,
        Phone = entity.Phone,
        Notes = entity.Notes,
        CreatedAt = entity.CreatedAt
    };
}
