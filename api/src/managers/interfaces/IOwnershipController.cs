using domain;

namespace managers;

public interface IOwnershipManager
{
    public List<Ownership> GetOwnerships(int[] carIds);
    public Task<core.Result<int>> CreateOwnership(Ownership ownership);
}
