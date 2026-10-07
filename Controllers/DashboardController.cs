using Gochs.ProtectiveStructures.DTOs.Dashboard;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.ProtectiveStructures.Controllers;

/// <summary>
/// Сводные показатели защитных сооружений ГО.
/// </summary>
[ApiController]
[Route("api/dashboard")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    /// <summary>
    /// Получить показатели Dashboard.
    /// </summary>
    /// <response code="200">Сводные показатели успешно рассчитаны.</response>
    [HttpGet]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardDto>> Get()
    {
        return Ok(await dashboardService.GetAsync());
    }
}
