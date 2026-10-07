using Gochs.ProtectiveStructures.Enums;

namespace Gochs.ProtectiveStructures.Entities;

public class Inspection
{
    public int Id { get; set; }
    public int ProtectiveStructureId { get; set; }
    public DateOnly InspectionDate { get; set; }
    public string InspectorName { get; set; } = null!;
    public ProtectiveStructureCondition ResultCondition { get; set; }
    public string? Findings { get; set; }
    public string? RequiredActions { get; set; }
    public DateOnly? NextInspectionDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public ProtectiveStructure ProtectiveStructure { get; set; } = null!;
}
