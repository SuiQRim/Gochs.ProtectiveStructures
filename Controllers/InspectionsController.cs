using Gochs.ProtectiveStructures.DTOs.Inspections;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.ProtectiveStructures.Controllers;

[ApiController]
[Route("api/inspections")]
public class InspectionsController : ControllerBase
{
    private readonly IInspectionService inspectionService;

    public InspectionsController(IInspectionService inspectionService)
    {
        this.inspectionService = inspectionService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<InspectionDto>>> GetAll()
    {
        return Ok(await inspectionService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InspectionDto>> GetById(int id)
    {
        return Ok(await inspectionService.GetByIdAsync(id));
    }

    [HttpGet("/api/protective-structures/{protectiveStructureId:int}/inspections")]
    public async Task<ActionResult<IReadOnlyCollection<InspectionDto>>> GetByProtectiveStructure(int protectiveStructureId)
    {
        return Ok(await inspectionService.GetByProtectiveStructureIdAsync(protectiveStructureId));
    }

    [HttpPost]
    public async Task<ActionResult<InspectionDto>> Create(CreateInspectionDto dto)
    {
        var result = await inspectionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
