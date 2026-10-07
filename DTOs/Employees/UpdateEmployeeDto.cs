using System.ComponentModel.DataAnnotations;

namespace Gochs.ProtectiveStructures.DTOs.Employees;

public class UpdateEmployeeDto
{
    [Required]
    [StringLength(50)]
    public string PersonnelNumber { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Department { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Position { get; set; } = null!;
}
