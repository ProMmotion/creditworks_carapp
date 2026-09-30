using data;
using models;

namespace services;

public class OwnershipService : IOwnershipService
{
    private readonly CarAppContext _context;
    public OwnershipService(CarAppContext context)
    {
        _context = context;
    }

    public List<domain.Ownership> GetOwnerships(int[] carIds)
    {
        return _context.Ownerships.Where(o => carIds.Contains(o.CarId)).Select(o => o.ToDomain()).ToList();
    }

    public async Task<int> CreateOwnership(domain.Ownership ownership)
    {
        var entry = _context.Ownerships.Add(Ownership.FromDomain(ownership));
        try
        {
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }
        catch
        {
            return 0;
        }
    }
}
