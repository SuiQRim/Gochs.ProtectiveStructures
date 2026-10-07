using Gochs.ProtectiveStructures.Data;
using Gochs.ProtectiveStructures.Entities;
using Gochs.ProtectiveStructures.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.ProtectiveStructures.Repositories.Implementations;

public class ProtectiveStructureRepository : IProtectiveStructureRepository
{
    private readonly AppDbContext context;

    public ProtectiveStructureRepository(AppDbContext context) => this.context = context;

    public Task<List<ProtectiveStructure>> GetAllAsync() =>
        context.ProtectiveStructures.AsNoTracking().OrderBy(x => x.Name).ToListAsync();

    public Task<ProtectiveStructure?> GetByIdAsync(int id) =>
        context.ProtectiveStructures.FirstOrDefaultAsync(x => x.Id == id);

    public Task<ProtectiveStructure?> GetDetailsAsync(int id) =>
        context.ProtectiveStructures.AsNoTracking()
            .Include(x => x.Employees)
            .Include(x => x.Inspections)
            .FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> ExistsAsync(int id) =>
        context.ProtectiveStructures.AnyAsync(x => x.Id == id);

    public Task<bool> RegistrationNumberExistsAsync(string registrationNumber, int? excludeId = null) =>
        context.ProtectiveStructures.AnyAsync(x =>
            x.RegistrationNumber == registrationNumber &&
            (!excludeId.HasValue || x.Id != excludeId.Value));

    public Task<int> GetAssignedEmployeeCountAsync(int id) =>
        context.Employees.CountAsync(x => x.ProtectiveStructureId == id);

    public Task<bool> HasInspectionsAsync(int id) =>
        context.Inspections.AnyAsync(x => x.ProtectiveStructureId == id);

    public async Task AddAsync(ProtectiveStructure entity)
    {
        context.ProtectiveStructures.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProtectiveStructure entity)
    {
        context.ProtectiveStructures.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProtectiveStructure entity)
    {
        context.ProtectiveStructures.Remove(entity);
        await context.SaveChangesAsync();
    }
}
