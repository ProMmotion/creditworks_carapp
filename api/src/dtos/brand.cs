using System.ComponentModel.DataAnnotations;
using domain;

namespace dtos;

public class CreateBrandDTO
{
    [Required]
    public string Name { get; set; }
    public string? ImgUrl { get; set; }

    public Brand ToDomain()
    {
        return new Brand { Name = Name, ImgUrl = ImgUrl ?? "" };
    }
}
