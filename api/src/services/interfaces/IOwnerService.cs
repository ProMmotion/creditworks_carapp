using domain;

namespace services;

public interface IOwnerService
{
    List<Owner> GetOwners();
    /// <summary>
    /// Creates a new owner record.
    /// </summary>
    /// <param name="owner">The owner record to create.</param>
    /// <returns>The ID of the created owner record.</returns>
    Task<int> CreateOwner(Owner owner);
}
