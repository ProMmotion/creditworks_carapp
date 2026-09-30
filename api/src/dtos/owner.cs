using System.ComponentModel.DataAnnotations;
using domain;

namespace dtos;

public class CreateOwnerDto
{
    [Required]
    public string Name { get; set; }

    public Owner ToDomain()
    {
        return new Owner { Name = Name };
    }
}
