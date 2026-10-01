using controllers;
using core;
using domain;
using dtos;
using managers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests;

public class CarsControllerTests
{
    [Fact]
    public void GetCars_ReturnsManagerResultAndMapsQueryDefaults()
    {
        var expected = new Paginated<Car> { Items = new() { new Car { Id = 7 } }, Total = 1 };
        var manager = new StubCarManager { Cars = expected };
        var controller = new CarsController(manager);

        var result = Assert.IsType<OkObjectResult>(controller.GetCars(new GetCarDTO { Page = 2 }));

        Assert.Same(expected, result.Value);
        Assert.Equal(2, manager.ReceivedQuery?.Page);
        Assert.Equal(10, manager.ReceivedQuery?.PageSize);
        Assert.Equal(SortOrder.Asc, manager.ReceivedQuery?.SortOrder);
    }

    [Fact]
    public async Task CreateCar_ReturnsOkWithIdWhenManagerSucceeds()
    {
        var manager = new StubCarManager { CreateResult = Result<int>.Success(23) };
        var controller = new CarsController(manager);
        var dto = new CreateCarDTO
        {
            BrandId = 1,
            ModelId = 2,
            NumberPlate = "ABC-123",
            Vin = "VIN-123",
            Year = 2020,
            Weight = 1500,
        };

        var result = Assert.IsType<OkObjectResult>(await controller.CreateCar(dto));

        Assert.Equal(23, result.Value!.GetType().GetProperty("id")!.GetValue(result.Value));
        Assert.Equal(1, manager.ReceivedCar?.BrandId);
        Assert.Equal(2, manager.ReceivedCar?.ModelId);
        Assert.Equal("ABC-123", manager.ReceivedCar?.NumberPlate);
        Assert.Equal("VIN-123", manager.ReceivedCar?.Vin);
        Assert.Equal(2020, manager.ReceivedCar?.Year);
        Assert.Equal(1500, manager.ReceivedCar?.Weight);
    }

    [Fact]
    public async Task CreateCar_ReturnsBadRequestWhenManagerFails()
    {
        var manager = new StubCarManager { CreateResult = Result<int>.Failure("cannot create") };
        var controller = new CarsController(manager);

        var result = Assert.IsType<BadRequestObjectResult>(await controller.CreateCar(new CreateCarDTO
        {
            BrandId = 1,
            ModelId = 2,
            Year = 2020,
            Weight = 1500,
        }));

        Assert.Equal("cannot create", result.Value!.GetType().GetProperty("error")!.GetValue(result.Value));
    }

    private sealed class StubCarManager : ICarManager
    {
        public Paginated<Car> Cars { get; set; } = new() { Items = new(), Total = 0 };
        public Result<int> CreateResult { get; set; } = Result<int>.Success(1);
        public CarQuery? ReceivedQuery { get; private set; }
        public Car? ReceivedCar { get; private set; }

        public Paginated<Car> GetCars(CarQuery query)
        {
            ReceivedQuery = query;
            return Cars;
        }

        public Task<Result<int>> CreateCar(Car car)
        {
            ReceivedCar = car;
            return Task.FromResult(CreateResult);
        }

        public Task<Result<int>> UpdateCar(Car car) => Task.FromResult(Result<int>.Success(1));
    }
}
