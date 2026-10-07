using Gochs.ProtectiveStructures.Enums;

namespace Gochs.ProtectiveStructures.Entities;

public class ProtectiveStructure
{
    public int Id { get; set; }
    public string RegistrationNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public ProtectiveStructureType Type { get; set; }
    public string Address { get; set; } = null!;
    public int Capacity { get; set; }
    public ProtectiveStructureCondition Condition { get; set; } = ProtectiveStructureCondition.Ready;
    public string? ResponsiblePerson { get; set; }
    public string? Phone { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
