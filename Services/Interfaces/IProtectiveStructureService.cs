using Gochs.ProtectiveStructures.DTOs.ProtectiveStructures;

namespace Gochs.ProtectiveStructures.Services.Interfaces;

public interface IProtectiveStructureService
{
    Task<IReadOnlyCollection<ProtectiveStructureDto>> GetAllAsync();
    Task<ProtectiveStructureDetailsDto> GetByIdAsync(int id);
    Task<ProtectiveStructureDto> CreateAsync(CreateProtectiveStructureDto dto);
    Task<ProtectiveStructureDto> UpdateAsync(int id, UpdateProtectiveStructureDto dto);
    Task DeleteAsync(int id);
}
