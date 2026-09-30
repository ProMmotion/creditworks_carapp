using domain;
using services;
using core;

namespace managers;

public class CarManager : ICarManager
{
    private readonly ICarService _service;
    private readonly IOwnershipService _ownershipService;
    public CarManager(ICarService service, IOwnershipService ownershipService)
    {
        _service = service;
        _ownershipService = ownershipService;
    }

    public core.Paginated<Car> GetCars(CarQuery query)
    {
        return _service.GetCars(query);
    }

    public async Task<core.Result<int>> CreateCar(Car car)
    {
        var id = await _service.CreateCar(car);
        if (id == 0) return core.Result<int>.Failure("Failed to create car");
        return core.Result<int>.Success(id);
    }

    public async Task<core.Result<int>> UpdateCar(Car car)
    {
        var affected = await _service.UpdateCar(car);
        if (affected == 0) return core.Result<int>.Failure("Failed to update car");
        return core.Result<int>.Success(affected);
    }
}
