using Microsoft.EntityFrameworkCore;
using models;

namespace data;

public static class DbInitializer
{
    public static async Task SeedAsync(CarAppContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // Apply migrations
        await context.Database.MigrateAsync();

        // Has data
        if (context.Brands.Any())
        {
            return;
        }

        // Seed
        var brands = new List<Brand>
        {
            new Brand { Id = 1, Name = "Mazda", ImgUrl = "https://upload.wikimedia.org/wikipedia/commons/4/43/Mazda_logo_2024_%28vertical%29.svg?utm_source=en.wikipedia.org&utm_campaign=index&utm_content=original" },
            new Brand { Id = 2, Name = "Mercedes", ImgUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4wTnXxbP-vynAPw-Hcr_v726Cq0s4SHwitz8an9hgAA&s" },
            new Brand { Id = 3, Name = "Honda", ImgUrl = "https://upload.wikimedia.org/wikipedia/commons/3/38/Honda.svg?utm_source=en.wikipedia.org&utm_campaign=index&utm_content=original" },
            new Brand { Id = 4, Name = "Ferrari", ImgUrl = "https://logos-marques.com/wp-content/uploads/2021/02/Ferrari-Logo.png" },
            new Brand { Id = 5, Name = "Toyota", ImgUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRUEDODeKz9eYMQsI__wjkqSs5EqQVMlGlnaEYx9tofXg&s=10" },
        };

        var owners = new List<Owner>
        {
            new Owner { Id = 1, Name = "Romain" },
            new Owner { Id = 2, Name = "Leo" },
            new Owner { Id = 3, Name = "Franck" },
            new Owner { Id = 4, Name = "Thibault" },
            new Owner { Id = 5, Name = "Sophie" },
            new Owner { Id = 6, Name = "Lea" },
        };

        var models = new List<Model>
        {
            // Mazda
            new Model { Id = 1, BrandId = 1, Name = "3" },
            // Mercedes
            new Model { Id = 2, BrandId = 2, Name = "A class" },
            // Honda
            new Model { Id = 3, BrandId = 3, Name = "Civic" },
            // Ferrari
            new Model { Id = 4, BrandId = 4, Name = "458" },
            // Toyota
            new Model { Id = 5, BrandId = 5, Name = "Prius" },
        };

        var categories = new List<Category>
        {
            new Category { Name = "Light", Icon = "1k", Filters = new domain.CarFilter { Weight = new core.Range(null, 1300) } },
            new Category { Name = "Medium", Icon = "2k", Filters = new domain.CarFilter { Weight = new core.Range(1300, 2400) } },
            new Category { Name = "Heavy", Icon = "3k", Filters = new domain.CarFilter { Weight = new core.Range(2400, null) } },
        };

        var cars = new List<Car>
        {
            new Car { Id = 1, BrandId = 1, ModelId = 1, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 2, BrandId = 2, ModelId = 2, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 3, BrandId = 3, ModelId = 3, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 4, BrandId = 4, ModelId = 4, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 5, BrandId = 5, ModelId = 5, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 6, BrandId = 1, ModelId = 1, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 7, BrandId = 2, ModelId = 2, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 8, BrandId = 3, ModelId = 3, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 9, BrandId = 4, ModelId = 4, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 10, BrandId = 5, ModelId = 5, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 11, BrandId = 1, ModelId = 1, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 12, BrandId = 2, ModelId = 2, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 13, BrandId = 3, ModelId = 3, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 14, BrandId = 4, ModelId = 4, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 15, BrandId = 5, ModelId = 5, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 16, BrandId = 1, ModelId = 1, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 17, BrandId = 2, ModelId = 2, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 18, BrandId = 3, ModelId = 3, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 19, BrandId = 4, ModelId = 4, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
            new Car { Id = 20, BrandId = 5, ModelId = 5, Year = 2000, Weight = 1234.56f, Vin = "", NumberPlate = "" },
        };

        var ownerships = new List<Ownership>
        {
            new Ownership { Id = 1, CarId = 1, OwnerId = 1 },
            new Ownership { Id = 2, CarId = 2, OwnerId = 2 },
            new Ownership { Id = 3, CarId = 3, OwnerId = 3 },
            new Ownership { Id = 4, CarId = 4, OwnerId = 4 },
            new Ownership { Id = 5, CarId = 5, OwnerId = 5 },
            new Ownership { Id = 6, CarId = 6, OwnerId = 1 },
            new Ownership { Id = 7, CarId = 7, OwnerId = 2 },
            new Ownership { Id = 8, CarId = 8, OwnerId = 3 },
            new Ownership { Id = 9, CarId = 9, OwnerId = 4 },
            new Ownership { Id = 10, CarId = 10, OwnerId = 5 },
            new Ownership { Id = 11, CarId = 11, OwnerId = 1 },
            new Ownership { Id = 12, CarId = 12, OwnerId = 2 },
            new Ownership { Id = 13, CarId = 13, OwnerId = 3 },
            new Ownership { Id = 14, CarId = 14, OwnerId = 4 },
            new Ownership { Id = 15, CarId = 15, OwnerId = 5 },
            new Ownership { Id = 16, CarId = 16, OwnerId = 1 },
            new Ownership { Id = 17, CarId = 17, OwnerId = 2 },
            new Ownership { Id = 18, CarId = 18, OwnerId = 3 },
            new Ownership { Id = 19, CarId = 19, OwnerId = 4 },
            new Ownership { Id = 20, CarId = 20, OwnerId = 5 },
        };

        await context.Brands.AddRangeAsync(brands);
        await context.Owners.AddRangeAsync(owners);
        await context.Models.AddRangeAsync(models);
        await context.Categories.AddRangeAsync(categories);
        await context.Cars.AddRangeAsync(cars);
        await context.Ownerships.AddRangeAsync(ownerships);

        await context.SaveChangesAsync();
    }
}
