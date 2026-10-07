using Gochs.ProtectiveStructures.Data;
using Gochs.ProtectiveStructures.DTOs.Dashboard;
using Gochs.ProtectiveStructures.Enums;
using Gochs.ProtectiveStructures.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.ProtectiveStructures.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext context;

    public DashboardService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<DashboardDto> GetAsync()
    {
        var structureCount = await context.ProtectiveStructures.CountAsync();
        var totalCapacity = await context.ProtectiveStructures.SumAsync(x => (int?)x.Capacity) ?? 0;
        var assignedEmployeeCount = await context.Employees.CountAsync(x => x.ProtectiveStructureId != null);
        var employeeCount = await context.Employees.CountAsync();

        var inspections = await context.Inspections
            .AsNoTracking()
            .OrderByDescending(x => x.InspectionDate)
            .ToListAsync();

        var today = DateOnly.FromDateTime(DateTime.Today);
        var overdueInspectionCount = inspections
            .GroupBy(x => x.ProtectiveStructureId)
            .Select(x => x.First())
            .Count(x => x.NextInspectionDate.HasValue && x.NextInspectionDate.Value < today);

        return new DashboardDto
        {
            ProtectiveStructureCount = structureCount,
            TotalCapacity = totalCapacity,
            AssignedEmployeeCount = assignedEmployeeCount,
            UnassignedEmployeeCount = employeeCount - assignedEmployeeCount,
            AvailableCapacity = totalCapacity - assignedEmployeeCount,
            ReadyCount = await context.ProtectiveStructures.CountAsync(x => x.Condition == ProtectiveStructureCondition.Ready),
            RequiresAttentionCount = await context.ProtectiveStructures.CountAsync(x => x.Condition == ProtectiveStructureCondition.RequiresAttention),
            NotReadyCount = await context.ProtectiveStructures.CountAsync(x => x.Condition == ProtectiveStructureCondition.NotReady),
            OverdueInspectionCount = overdueInspectionCount
        };
    }
}
