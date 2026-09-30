using domain;

namespace services;

public interface IOwnershipService
{
    List<Ownership> GetOwnerships(int[] carIds);
    /// <summary>
    /// Creates a new ownership record.
    /// </summary>
    /// <param name="ownership">The ownership record to create.</param>
    /// <returns>The ID of the created ownership record.</returns>
    Task<int> CreateOwnership(Ownership ownership);
}
