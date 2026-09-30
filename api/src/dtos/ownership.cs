using domain;

namespace dtos;

public record CreateOwnershipDto
{
    public int OwnerId { get; init; }
    public int CarId { get; init; }

    public Ownership ToDomain()
    {
        return new Ownership
        {
            OwnerId = OwnerId,
            CarId = CarId,
        };
    }
}
