using Gochs.ProtectiveStructures.DTOs.ProtectiveStructures;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.ProtectiveStructures.Controllers;

[ApiController]
[Route("api/protective-structures")]
public class ProtectiveStructuresController : ControllerBase
{
    private readonly IProtectiveStructureService protectiveStructureService;

    public ProtectiveStructuresController(IProtectiveStructureService protectiveStructureService)
    {
        this.protectiveStructureService = protectiveStructureService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProtectiveStructureDto>>> GetAll()
    {
        return Ok(await protectiveStructureService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProtectiveStructureDetailsDto>> GetById(int id)
    {
        return Ok(await protectiveStructureService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<ProtectiveStructureDto>> Create(CreateProtectiveStructureDto dto)
    {
        var result = await protectiveStructureService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProtectiveStructureDto>> Update(int id, UpdateProtectiveStructureDto dto)
    {
        return Ok(await protectiveStructureService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await protectiveStructureService.DeleteAsync(id);
        return NoContent();
    }
}
