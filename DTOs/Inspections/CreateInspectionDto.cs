using System.ComponentModel.DataAnnotations;
using Gochs.ProtectiveStructures.Enums;

namespace Gochs.ProtectiveStructures.DTOs.Inspections;

public class CreateInspectionDto
{
    [Range(1, int.MaxValue)]
    public int ProtectiveStructureId { get; set; }

    [Required]
    public DateOnly? InspectionDate { get; set; }

    [Required]
    [StringLength(200)]
    public string InspectorName { get; set; } = null!;

    [Required]
    public ProtectiveStructureCondition? ResultCondition { get; set; }

    [StringLength(1000)]
    public string? Findings { get; set; }

    [StringLength(1000)]
    public string? RequiredActions { get; set; }

    public DateOnly? NextInspectionDate { get; set; }
}
