using Gochs.ProtectiveStructures.DTOs.Inspections;
using Gochs.ProtectiveStructures.Entities;
using Gochs.ProtectiveStructures.Exceptions;
using Gochs.ProtectiveStructures.Mappings;
using Gochs.ProtectiveStructures.Repositories.Interfaces;
using Gochs.ProtectiveStructures.Services.Interfaces;

namespace Gochs.ProtectiveStructures.Services.Implementations;

public class InspectionService : IInspectionService
{
    private readonly IInspectionRepository inspectionRepository;
    private readonly IProtectiveStructureRepository protectiveStructureRepository;

    public InspectionService(
        IInspectionRepository inspectionRepository,
        IProtectiveStructureRepository protectiveStructureRepository)
    {
        this.inspectionRepository = inspectionRepository;
        this.protectiveStructureRepository = protectiveStructureRepository;
    }

    public async Task<IReadOnlyCollection<InspectionDto>> GetAllAsync() =>
        (await inspectionRepository.GetAllAsync()).Select(x => x.ToDto()).ToList();

    public async Task<IReadOnlyCollection<InspectionDto>> GetByProtectiveStructureIdAsync(int protectiveStructureId)
    {
        if (!await protectiveStructureRepository.ExistsAsync(protectiveStructureId))
            throw new NotFoundException($"Защитное сооружение с id {protectiveStructureId} не найдено.");

        return (await inspectionRepository.GetByProtectiveStructureIdAsync(protectiveStructureId))
            .Select(x => x.ToDto())
            .ToList();
    }

    public async Task<InspectionDto> GetByIdAsync(int id)
    {
        var entity = await inspectionRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Проверка с id {id} не найдена.");

        return entity.ToDto();
    }

    public async Task<InspectionDto> CreateAsync(CreateInspectionDto dto)
    {
        var structure = await protectiveStructureRepository.GetByIdAsync(dto.ProtectiveStructureId)
            ?? throw new NotFoundException($"Защитное сооружение с id {dto.ProtectiveStructureId} не найдено.");

        var inspectionDate = dto.InspectionDate!.Value;
        var today = DateOnly.FromDateTime(DateTime.Today);

        if (inspectionDate > today)
            throw new ValidationException("Дата проведённой проверки не может быть в будущем.");

        if (dto.NextInspectionDate.HasValue && dto.NextInspectionDate.Value <= inspectionDate)
            throw new ValidationException("Дата следующей проверки должна быть позже даты текущей проверки.");

        var entity = new Inspection
        {
            ProtectiveStructureId = dto.ProtectiveStructureId,
            InspectionDate = inspectionDate,
            InspectorName = RequireText(dto.InspectorName, "Проверяющий"),
            ResultCondition = dto.ResultCondition!.Value,
            Findings = Normalize(dto.Findings),
            RequiredActions = Normalize(dto.RequiredActions),
            NextInspectionDate = dto.NextInspectionDate
        };

        structure.Condition = entity.ResultCondition;

        await inspectionRepository.AddAsync(entity);
        return entity.ToDto();
    }

    private static string RequireText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} не может быть пустым.");

        return value.Trim();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
