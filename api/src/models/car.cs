namespace models;

public class Car
{
    public int Id { get; set; }
    public Brand Brand { get; set; }
    public int BrandId { get; set; }
    public Model Model { get; set; }
    public int ModelId { get; set; }
    public string NumberPlate { get; set; }
    public Ownership Ownership { get; set; }
    public string Vin { get; set; }
    public float Weight { get; set; }
    public int Year { get; set; }

    public static Car FromDomain(domain.Car car)
    {
        return new Car
        {
            Id = car.Id ?? 0,
            BrandId = car.BrandId,
            ModelId = car.ModelId,
            NumberPlate = car.NumberPlate,
            Vin = car.Vin,
            Weight = car.Weight,
            Year = car.Year,
        };
    }

    public domain.Car ToDomain()
    {
        return new domain.Car
        {
            Id = Id,
            BrandId = BrandId,
            ModelId = ModelId,
            NumberPlate = NumberPlate,
            Vin = Vin,
            Weight = Weight,
            Year = Year,
        };
    }
}
