using data;
using models;

namespace services;

public class OwnerService : IOwnerService
{
    private readonly CarAppContext _context;
    public OwnerService(CarAppContext context)
    {
        _context = context;
    }

    public List<domain.Owner> GetOwners()
    {
        var owners = _context.Owners.ToList();
        return owners.Select(o => o.ToDomain()).ToList();
    }

    public async Task<int> CreateOwner(domain.Owner owner)
    {
        var entry = _context.Owners.Add(Owner.FromDomain(owner));
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
