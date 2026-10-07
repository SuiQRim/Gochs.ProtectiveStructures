using Gochs.ProtectiveStructures.DTOs.Dashboard;

namespace Gochs.ProtectiveStructures.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetAsync();
}
