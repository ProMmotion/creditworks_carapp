using data;
using domain;
using Microsoft.EntityFrameworkCore;
using services;
using Xunit;

namespace tests;

public class CarServiceTests : IDisposable
{
    private readonly CarAppContext _context = TestDbContext.Create();
    private readonly CarService _service;

    public CarServiceTests()
    {
        SeedCarData();
        _service = new CarService(_context);
    }

    [Fact]
    public void GetCars_WithYearFilter_ReturnsMatchingCars()
    {
        var result = _service.GetCars(new CarQuery
        {
            Page = 1,
            PageSize = 10,
            Filters = new CarFilter { Year = new core.Range { From = 2021, To = 2023 } },
        });

        Assert.Equal(1, result.Total);
        Assert.Equal(2022, Assert.Single(result.Items).Year);
    }

    [Fact]
    public void GetCars_UsesSortingAndPageOffset()
    {
        _context.Cars.AddRange(
            new models.Car { Id = 3, BrandId = 1, ModelId = 1, Year = 2018, Weight = 1300, NumberPlate = "OLD-001", Vin = "VIN3" },
            new models.Car { Id = 4, BrandId = 1, ModelId = 2, Year = 2024, Weight = 1900, NumberPlate = "NEW-001", Vin = "VIN4" });
        _context.SaveChanges();

        var result = _service.GetCars(new CarQuery
        {
            Page = 2,
            PageSize = 1,
            SortBy = "year",
            SortOrder = core.SortOrder.Asc,
        });

        Assert.Equal(4, result.Total);
        Assert.Equal(2020, Assert.Single(result.Items).Year);
    }

    [Fact]
    public void GetCars_CombinesBrandModelAndWeightFilters()
    {
        var result = _service.GetCars(new CarQuery
        {
            Page = 1,
            PageSize = 10,
            Filters = new CarFilter
            {
                BrandId = 1,
                ModelId = 2,
                Weight = new core.Range(1700, 1900),
            },
        });

        Assert.Equal(1, result.Total);
        Assert.Equal(2, Assert.Single(result.Items).ModelId);
    }

    [Fact]
    public async Task CreateAndUpdateCar_PersistsChangesAndRejectsMissingId()
    {
        var car = new Car
        {
            BrandId = 1,
            ModelId = 1,
            NumberPlate = "ABC",
            Vin = "VIN-ABC",
            Weight = 1400,
            Year = 2022,
        };

        var id = await _service.CreateCar(car);
        Assert.True(id > 0);
        Assert.Equal(0, await _service.UpdateCar(new Car { BrandId = 1, ModelId = 1 }));

        _context.ChangeTracker.Clear();
        car.Id = id;
        car.Year = 2023;
        Assert.Equal(1, await _service.UpdateCar(car));
        Assert.Equal(2023, _service.GetCars(new CarQuery { Page = 1, PageSize = 10 }).Items.Single(c => c.Id == id).Year);
    }

    private void SeedCarData()
    {
        _context.Brands.Add(new models.Brand { Id = 1, Name = "Audi", ImgUrl = "" });
        _context.SaveChanges();
        _context.Models.AddRange(
            new models.Model { Id = 1, Name = "A3", BrandId = 1 },
            new models.Model { Id = 2, Name = "A6", BrandId = 1 });
        _context.SaveChanges();
        _context.Cars.AddRange(
            new models.Car { Id = 1, BrandId = 1, ModelId = 1, Year = 2020, Weight = 1500, NumberPlate = "ABC-123", Vin = "VIN1" },
            new models.Car { Id = 2, BrandId = 1, ModelId = 2, Year = 2022, Weight = 1800, NumberPlate = "XYZ-789", Vin = "VIN2" });
        _context.SaveChanges();
    }

    public void Dispose()
    {
        TestDbContext.Dispose(_context);
    }
}
