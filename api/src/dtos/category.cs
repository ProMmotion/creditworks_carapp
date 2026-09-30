using System.ComponentModel.DataAnnotations;
using core;

namespace dtos;

public class CreateCategoryDTO
{
    [Required]
    public GetCarFiltersDTO Filters { get; set; }
    public string? Icon { get; set; }
    [Required]
    public string Name { get; set; }

    public domain.Category ToDomain()
    {
        return new domain.Category
        {
            Filters = Filters.ToDomain(),
            Icon = Icon ?? "",
            Name = Name,
        };
    }
}

public class UpdateCategoryDTO
{
    public GetCarFiltersDTO? Filters { get; set; }
    public string? Icon { get; set; }
    public string? Name { get; set; }

    public domain.Category ToDomain()
    {
        return new domain.Category
        {
            Filters = Filters?.ToDomain() ?? new domain.CarFilter(),
            Icon = Icon ?? "",
            Name = Name ?? "",
        };
    }
}
