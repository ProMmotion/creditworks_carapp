namespace models;

public class Model
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string Name { get; set; }

    public static Model FromDomain(domain.Model model)
    {
        return new Model
        {
            Id = model.Id ?? 0,
            BrandId = model.BrandId,
            Name = model.Name,
        };
    }

    public domain.Model ToDomain()
    {
        return new domain.Model
        {
            Id = Id,
            BrandId = BrandId,
            Name = Name,
        };
    }
}
