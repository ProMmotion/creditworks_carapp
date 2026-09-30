using Microsoft.EntityFrameworkCore;

namespace models;

public class Brand
{
    public int Id { get; set; }
    public string ImgUrl { get; set; }
    public string Name { get; set; }

    public static Brand FromDomain(domain.Brand brand)
    {
        return new Brand { Id = brand.Id ?? 0, Name = brand.Name, ImgUrl = brand.ImgUrl };
    }

    public domain.Brand ToDomain()
    {
        return new domain.Brand { Id = Id, Name = Name, ImgUrl = ImgUrl };
    }
}
