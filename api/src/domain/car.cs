
namespace domain;

public class Car
{
    public int? Id { get; set; }
    public int BrandId { get; set; }
    public int ModelId { get; set; }
    public string NumberPlate { get; set; }
    public string Vin { get; set; }
    public float Weight { get; set; }
    public int Year { get; set; }
}

public class CarQuery
{
    public CarFilter? Filters { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public core.SortOrder? SortOrder { get; set; }
}

public class CarFilter
{
    public int? BrandId { get; set; }
    public int? ModelId { get; set; }
    public core.Range? Weight { get; set; }
    public core.Range? Year { get; set; }
}
