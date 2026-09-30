using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace models;

public class Category
{
    [NotMapped]
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        IncludeFields = true
    };

    public int Id { get; set; }

    [NotMapped]
    public domain.CarFilter Filters { get; set; }
    public string FiltersJson
    {
        get => JsonSerializer.Serialize(Filters, JsonOptions);
        set => Filters = string.IsNullOrEmpty(value)
            ? new domain.CarFilter()
            : JsonSerializer.Deserialize<domain.CarFilter>(value, JsonOptions) ?? new domain.CarFilter();
    }
    public string Icon { get; set; }
    public string Name { get; set; }

    public static Category FromDomain(domain.Category category)
    {
        return new Category
        {
            Id = category.Id ?? 0,
            Filters = category.Filters,
            Icon = category.Icon,
            Name = category.Name,
        };
    }

    public domain.Category ToDomain()
    {
        return new domain.Category
        {
            Id = Id,
            Filters = Filters,
            Icon = Icon,
            Name = Name,
        };
    }
}
