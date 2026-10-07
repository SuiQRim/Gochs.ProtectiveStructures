using Gochs.ProtectiveStructures.Enums;

namespace Gochs.ProtectiveStructures.DTOs.ProtectiveStructures;

public class ProtectiveStructureDto
{
    public int Id { get; set; }
    public string RegistrationNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public ProtectiveStructureType Type { get; set; }
    public string Address { get; set; } = null!;
    public int Capacity { get; set; }
    public int AssignedEmployeeCount { get; set; }
    public int AvailableCapacity { get; set; }
    public ProtectiveStructureCondition Condition { get; set; }
    public string? ResponsiblePerson { get; set; }
    public string? Phone { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
