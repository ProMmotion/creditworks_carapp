using domain;

namespace managers;

public interface ICarManager
{
    public core.Paginated<Car> GetCars(CarQuery filter);
    public Task<core.Result<int>> CreateCar(Car car);
    public Task<core.Result<int>> UpdateCar(Car car);
}
