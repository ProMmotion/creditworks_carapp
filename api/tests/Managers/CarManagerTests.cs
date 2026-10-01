using domain;
using managers;
using services;
using Xunit;

namespace tests;

public class CarManagerTests
{
    [Theory]
    [InlineData(0, false, null)]
    [InlineData(23, true, 23)]
    public async Task CreateCar_ConvertsServiceIdToResult(int serviceId, bool expectedSuccess, int? expectedId)
    {
        var service = new StubCarService { CreateResult = serviceId };
        var manager = new CarManager(service, new StubOwnershipService());

        var result = await manager.CreateCar(new Car());

        Assert.Equal(expectedSuccess, result.IsSuccess);
        if (expectedSuccess) Assert.Equal(expectedId, result.Value);
        else Assert.Equal("Failed to create car", result.Error);
    }

    [Theory]
    [InlineData(0, false, null)]
    [InlineData(1, true, 1)]
    public async Task UpdateCar_ConvertsAffectedRowsToResult(int affectedRows, bool expectedSuccess, int? expectedValue)
    {
        var service = new StubCarService { UpdateResult = affectedRows };
        var manager = new CarManager(service, new StubOwnershipService());

        var result = await manager.UpdateCar(new Car { Id = 1 });

        Assert.Equal(expectedSuccess, result.IsSuccess);
        if (expectedSuccess) Assert.Equal(expectedValue, result.Value);
        else Assert.Equal("Failed to update car", result.Error);
    }

    private sealed class StubCarService : ICarService
    {
        public int CreateResult { get; init; }
        public int UpdateResult { get; init; }
        public core.Paginated<Car> GetCars(CarQuery query) => new() { Items = new(), Total = 0 };
        public Task<int> CreateCar(Car car) => Task.FromResult(CreateResult);
        public Task<int> UpdateCar(Car car) => Task.FromResult(UpdateResult);
    }

    private sealed class StubOwnershipService : IOwnershipService
    {
        public List<Ownership> GetOwnerships(int[] carIds) => new();
        public Task<int> CreateOwnership(Ownership ownership) => Task.FromResult(0);
    }
}
