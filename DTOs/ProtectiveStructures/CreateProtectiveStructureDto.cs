using System.ComponentModel.DataAnnotations;
using Gochs.ProtectiveStructures.Enums;

namespace Gochs.ProtectiveStructures.DTOs.ProtectiveStructures;

public class CreateProtectiveStructureDto
{
    [Required]
    [StringLength(100)]
    public string RegistrationNumber { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    public ProtectiveStructureType? Type { get; set; }

    [Required]
    [StringLength(300)]
    public string Address { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

    [StringLength(200)]
    public string? ResponsiblePerson { get; set; }

    [StringLength(30)]
    public string? Phone { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }
}
