namespace Gochs.ProtectiveStructures.Entities;

public class Employee
{
    public int Id { get; set; }
    public string PersonnelNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Department { get; set; } = null!;
    public string Position { get; set; } = null!;
    public int? ProtectiveStructureId { get; set; }
    public DateTime CreatedAt { get; set; }

    public ProtectiveStructure? ProtectiveStructure { get; set; }
}
