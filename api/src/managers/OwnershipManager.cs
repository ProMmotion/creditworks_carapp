using domain;
using services;

namespace managers;

public class OwnershipManager : IOwnershipManager
{
    private readonly IOwnershipService _service;
    public OwnershipManager(IOwnershipService service)
    {
        _service = service;
    }

    public List<Ownership> GetOwnerships(int[] carIds)
    {
        return _service.GetOwnerships(carIds);
    }

    public async Task<core.Result<int>> CreateOwnership(Ownership ownership)
    {
        int id = await _service.CreateOwnership(ownership);
        if (id == 0) return core.Result<int>.Failure("Failed to create ownership");
        return core.Result<int>.Success(id);
    }
}
