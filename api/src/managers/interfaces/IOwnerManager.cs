using domain;

namespace managers;

public interface IOwnerManager
{
    public List<Owner> GetOwners();
    public Task<core.Result<int>> CreateOwner(Owner owner);
}
