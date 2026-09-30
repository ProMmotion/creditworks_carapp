namespace models;

public class Owner
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Ownership Ownership { get; set; }

    public static Owner FromDomain(domain.Owner owner)
    {
        return new Owner
        {
            Id = owner.Id ?? 0,
            Name = owner.Name,
        };
    }

    public domain.Owner ToDomain()
    {
        return new domain.Owner
        {
            Id = Id,
            Name = Name,
        };
    }
}
