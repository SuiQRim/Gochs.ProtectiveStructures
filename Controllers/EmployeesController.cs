using Gochs.ProtectiveStructures.DTOs.Common;
using Gochs.ProtectiveStructures.DTOs.Employees;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.ProtectiveStructures.Controllers;

/// <summary>
/// Управление сотрудниками и их распределением по защитным сооружениям.
/// </summary>
[ApiController]
[Route("api/employees")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    /// <summary>
    /// Получить список всех сотрудников.
    /// </summary>
    /// <response code="200">Список сотрудников успешно получен.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetAll()
    {
        return Ok(await employeeService.GetAllAsync());
    }

    /// <summary>
    /// Получить сотрудника по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <response code="200">Сотрудник найден.</response>
    /// <response code="404">Сотрудник не найден.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        return Ok(await employeeService.GetByIdAsync(id));
    }

    /// <summary>
    /// Получить сотрудников, распределённых в конкретное защитное сооружение.
    /// </summary>
    /// <param name="protectiveStructureId">Идентификатор сооружения.</param>
    /// <response code="200">Список сотрудников успешно получен.</response>
    /// <response code="404">Сооружение не найдено.</response>
    [HttpGet("/api/protective-structures/{protectiveStructureId:int}/employees")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetByProtectiveStructure(int protectiveStructureId)
    {
        return Ok(await employeeService.GetByProtectiveStructureIdAsync(protectiveStructureId));
    }

    /// <summary>
    /// Создать сотрудника.
    /// </summary>
    /// <param name="dto">Данные сотрудника.</param>
    /// <response code="201">Сотрудник создан.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="409">Табельный номер уже используется.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var result = await employeeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновить данные сотрудника.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <param name="dto">Новые данные сотрудника.</param>
    /// <response code="200">Сотрудник обновлён.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Сотрудник не найден.</response>
    /// <response code="409">Табельный номер уже используется.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeeDto>> Update(int id, UpdateEmployeeDto dto)
    {
        return Ok(await employeeService.UpdateAsync(id, dto));
    }

    /// <summary>
    /// Изменить распределение сотрудника по защитным сооружениям.
    /// </summary>
    /// <remarks>
    /// Для снятия распределения передайте protectiveStructureId = null.
    /// Назначение в NotReady или полностью заполненное сооружение запрещено.
    /// </remarks>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <param name="dto">Новое назначение сотрудника.</param>
    /// <response code="200">Распределение обновлено.</response>
    /// <response code="404">Сотрудник или сооружение не найдено.</response>
    /// <response code="409">Сооружение не готово или в нём нет свободных мест.</response>
    [HttpPatch("{id:int}/assignment")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeeDto>> UpdateAssignment(int id, UpdateEmployeeAssignmentDto dto)
    {
        return Ok(await employeeService.UpdateAssignmentAsync(id, dto));
    }

    /// <summary>
    /// Удалить сотрудника.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <response code="204">Сотрудник удалён.</response>
    /// <response code="404">Сотрудник не найден.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await employeeService.DeleteAsync(id);
        return NoContent();
    }
}
