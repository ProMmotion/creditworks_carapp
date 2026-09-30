using System.ComponentModel.DataAnnotations;

namespace dtos;

public class CreateModelDto
{
    [Required]
    public int BrandId { get; set; }
    [Required]
    public string Name { get; set; }

    public domain.Model ToDomain()
    {
        return new domain.Model
        {
            BrandId = BrandId,
            Name = Name,
        };
    }
}
