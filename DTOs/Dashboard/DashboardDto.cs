namespace Gochs.ProtectiveStructures.DTOs.Dashboard;

public class DashboardDto
{
    public int ProtectiveStructureCount { get; set; }
    public int TotalCapacity { get; set; }
    public int AssignedEmployeeCount { get; set; }
    public int UnassignedEmployeeCount { get; set; }
    public int AvailableCapacity { get; set; }

    public int ReadyCount { get; set; }
    public int RequiresAttentionCount { get; set; }
    public int NotReadyCount { get; set; }

    public int OverdueInspectionCount { get; set; }
}
