using Gochs.ProtectiveStructures.DTOs.Employees;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.ProtectiveStructures.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetAll()
    {
        return Ok(await employeeService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        return Ok(await employeeService.GetByIdAsync(id));
    }

    [HttpGet("/api/protective-structures/{protectiveStructureId:int}/employees")]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetByProtectiveStructure(int protectiveStructureId)
    {
        return Ok(await employeeService.GetByProtectiveStructureIdAsync(protectiveStructureId));
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var result = await employeeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, UpdateEmployeeDto dto)
    {
        return Ok(await employeeService.UpdateAsync(id, dto));
    }

    [HttpPatch("{id:int}/assignment")]
    public async Task<ActionResult<EmployeeDto>> UpdateAssignment(int id, UpdateEmployeeAssignmentDto dto)
    {
        return Ok(await employeeService.UpdateAssignmentAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await employeeService.DeleteAsync(id);
        return NoContent();
    }
}
