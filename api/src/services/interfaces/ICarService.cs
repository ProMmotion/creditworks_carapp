using domain;

namespace services;

public interface ICarService
{
    core.Paginated<Car> GetCars(CarQuery query);
    /// <summary>
    /// Creates a new car.
    /// </summary>
    /// <param name="car">The car to create.</param>
    /// <returns>The ID of the created car. 0 if creation failed.</returns>
    Task<int> CreateCar(Car car);
    /// <summary>
    /// Updates an existing car.
    /// </summary>
    /// <param name="car">The car to update.</param>
    /// <returns>The number of rows affected. 0 if update failed.</returns>
    Task<int> UpdateCar(Car car);
}
