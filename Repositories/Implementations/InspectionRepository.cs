using Gochs.ProtectiveStructures.Data;
using Gochs.ProtectiveStructures.Entities;
using Gochs.ProtectiveStructures.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.ProtectiveStructures.Repositories.Implementations;

public class InspectionRepository : IInspectionRepository
{
    private readonly AppDbContext context;

    public InspectionRepository(AppDbContext context) => this.context = context;

    public Task<List<Inspection>> GetAllAsync() =>
        context.Inspections.AsNoTracking()
            .OrderByDescending(x => x.InspectionDate)
            .ToListAsync();

    public Task<List<Inspection>> GetByProtectiveStructureIdAsync(int protectiveStructureId) =>
        context.Inspections.AsNoTracking()
            .Where(x => x.ProtectiveStructureId == protectiveStructureId)
            .OrderByDescending(x => x.InspectionDate)
            .ToListAsync();

    public Task<Inspection?> GetByIdAsync(int id) =>
        context.Inspections.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Inspection entity)
    {
        context.Inspections.Add(entity);
        await context.SaveChangesAsync();
    }
}
