using domain;
using services;

namespace managers;

public class OwnerManager : IOwnerManager
{
    private readonly IOwnerService _service;
    public OwnerManager(IOwnerService service)
    {
        _service = service;
    }

    public List<Owner> GetOwners()
    {
        return _service.GetOwners();
    }

    public async Task<core.Result<int>> CreateOwner(Owner owner)
    {
        int id = await _service.CreateOwner(owner);
        if (id == 0) return core.Result<int>.Failure("Failed to create owner");
        return core.Result<int>.Success(id);
    }
}
