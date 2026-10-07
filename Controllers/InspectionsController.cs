using Gochs.ProtectiveStructures.DTOs.Common;
using Gochs.ProtectiveStructures.DTOs.Inspections;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.ProtectiveStructures.Controllers;

/// <summary>
/// Управление проверками защитных сооружений.
/// </summary>
[ApiController]
[Route("api/inspections")]
[Produces("application/json")]
public class InspectionsController : ControllerBase
{
    private readonly IInspectionService inspectionService;

    public InspectionsController(IInspectionService inspectionService)
    {
        this.inspectionService = inspectionService;
    }

    /// <summary>
    /// Получить историю всех проверок.
    /// </summary>
    /// <response code="200">История проверок успешно получена.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InspectionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<InspectionDto>>> GetAll()
    {
        return Ok(await inspectionService.GetAllAsync());
    }

    /// <summary>
    /// Получить проверку по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор проверки.</param>
    /// <response code="200">Проверка найдена.</response>
    /// <response code="404">Проверка не найдена.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InspectionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionDto>> GetById(int id)
    {
        return Ok(await inspectionService.GetByIdAsync(id));
    }

    /// <summary>
    /// Получить историю проверок конкретного сооружения.
    /// </summary>
    /// <param name="protectiveStructureId">Идентификатор сооружения.</param>
    /// <response code="200">История проверок успешно получена.</response>
    /// <response code="404">Сооружение не найдено.</response>
    [HttpGet("/api/protective-structures/{protectiveStructureId:int}/inspections")]
    [ProducesResponseType(typeof(IEnumerable<InspectionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<InspectionDto>>> GetByProtectiveStructure(int protectiveStructureId)
    {
        return Ok(await inspectionService.GetByProtectiveStructureIdAsync(protectiveStructureId));
    }

    /// <summary>
    /// Зарегистрировать новую проверку сооружения.
    /// </summary>
    /// <remarks>
    /// Результат проверки автоматически становится текущим состоянием сооружения.
    /// Дата проверки не может быть в будущем, а следующая дата должна быть позже текущей.
    /// </remarks>
    /// <param name="dto">Данные проверки.</param>
    /// <response code="201">Проверка зарегистрирована.</response>
    /// <response code="400">Переданы некорректные даты или другие данные.</response>
    /// <response code="404">Защитное сооружение не найдено.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(InspectionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionDto>> Create(CreateInspectionDto dto)
    {
        var result = await inspectionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
