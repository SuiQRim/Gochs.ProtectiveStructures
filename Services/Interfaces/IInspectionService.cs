using Gochs.ProtectiveStructures.DTOs.Inspections;

namespace Gochs.ProtectiveStructures.Services.Interfaces;

public interface IInspectionService
{
    Task<IReadOnlyCollection<InspectionDto>> GetAllAsync();
    Task<IReadOnlyCollection<InspectionDto>> GetByProtectiveStructureIdAsync(int protectiveStructureId);
    Task<InspectionDto> GetByIdAsync(int id);
    Task<InspectionDto> CreateAsync(CreateInspectionDto dto);
}
