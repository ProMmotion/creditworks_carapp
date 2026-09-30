using System.ComponentModel.DataAnnotations;
using domain;

namespace dtos;

public class CreateCarDTO
{
    [Required]
    public int BrandId { get; set; }
    [Required]
    public int ModelId { get; set; }
    public string NumberPlate { get; set; } = "";
    public string Vin { get; set; } = "";
    [Required]
    public float Weight { get; set; }
    [Required]
    public int Year { get; set; }

    public Car ToDomain()
    {
        return new Car
        {
            BrandId = BrandId,
            ModelId = ModelId,
            NumberPlate = NumberPlate,
            Vin = Vin,
            Weight = Weight,
            Year = Year,
        };
    }
}

public class GetCarDTO
{
    public GetCarFiltersDTO? Filters { get; set; }
    public int Page { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public core.SortOrder? SortOrder { get; set; }

    public CarQuery ToDomain()
    {
        return new CarQuery
        {
            Filters = Filters?.ToDomain(),
            Page = Page,
            PageSize = PageSize ?? 10,
            SortBy = SortBy,
            SortOrder = SortOrder ?? core.SortOrder.Asc,
        };
    }
}

public class GetCarFiltersDTO
{
    public int? BrandId { get; set; }
    public int? ModelId { get; set; }
    public core.Range? Weight { get; set; }
    public core.Range? Year { get; set; }

    public CarFilter ToDomain()
    {
        return new CarFilter
        {
            BrandId = BrandId,
            ModelId = ModelId,
            Weight = Weight,
            Year = Year,
        };
    }
}
