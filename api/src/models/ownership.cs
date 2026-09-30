namespace models;

public class Ownership
{
    public int Id { get; set; }
    public Car Car { get; set; }
    public int CarId { get; set; }
    public Owner Owner { get; set; }
    public int OwnerId { get; set; }

    public static Ownership FromDomain(domain.Ownership ownership)
    {
        return new Ownership
        {
            Id = ownership.Id ?? 0,
            CarId = ownership.CarId,
            OwnerId = ownership.OwnerId,
        };
    }

    public domain.Ownership ToDomain()
    {
        return new domain.Ownership
        {
            Id = Id,
            CarId = CarId,
            OwnerId = OwnerId,
        };
    }
}
