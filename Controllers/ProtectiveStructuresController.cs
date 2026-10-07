using Gochs.ProtectiveStructures.DTOs.Common;
using Gochs.ProtectiveStructures.DTOs.ProtectiveStructures;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.ProtectiveStructures.Controllers;

/// <summary>
/// Управление защитными сооружениями гражданской обороны.
/// </summary>
[ApiController]
[Route("api/protective-structures")]
[Produces("application/json")]
public class ProtectiveStructuresController : ControllerBase
{
    private readonly IProtectiveStructureService protectiveStructureService;

    public ProtectiveStructuresController(IProtectiveStructureService protectiveStructureService)
    {
        this.protectiveStructureService = protectiveStructureService;
    }

    /// <summary>
    /// Получить список всех защитных сооружений.
    /// </summary>
    /// <response code="200">Список сооружений успешно получен.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProtectiveStructureDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProtectiveStructureDto>>> GetAll()
    {
        return Ok(await protectiveStructureService.GetAllAsync());
    }

    /// <summary>
    /// Получить подробную карточку защитного сооружения.
    /// </summary>
    /// <param name="id">Идентификатор сооружения.</param>
    /// <response code="200">Сооружение найдено.</response>
    /// <response code="404">Сооружение не найдено.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProtectiveStructureDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProtectiveStructureDetailsDto>> GetById(int id)
    {
        return Ok(await protectiveStructureService.GetByIdAsync(id));
    }

    /// <summary>
    /// Создать защитное сооружение.
    /// </summary>
    /// <param name="dto">Данные нового сооружения.</param>
    /// <response code="201">Сооружение создано.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="409">Регистрационный номер уже используется.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProtectiveStructureDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProtectiveStructureDto>> Create(CreateProtectiveStructureDto dto)
    {
        var result = await protectiveStructureService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновить защитное сооружение.
    /// </summary>
    /// <param name="id">Идентификатор сооружения.</param>
    /// <param name="dto">Новые данные сооружения.</param>
    /// <response code="200">Сооружение обновлено.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Сооружение не найдено.</response>
    /// <response code="409">Нарушено ограничение вместимости или уникальности регистрационного номера.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProtectiveStructureDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProtectiveStructureDto>> Update(int id, UpdateProtectiveStructureDto dto)
    {
        return Ok(await protectiveStructureService.UpdateAsync(id, dto));
    }

    /// <summary>
    /// Удалить защитное сооружение.
    /// </summary>
    /// <param name="id">Идентификатор сооружения.</param>
    /// <response code="204">Сооружение удалено.</response>
    /// <response code="404">Сооружение не найдено.</response>
    /// <response code="409">Удаление запрещено из-за распределённых сотрудников или истории проверок.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        await protectiveStructureService.DeleteAsync(id);
        return NoContent();
    }
}
