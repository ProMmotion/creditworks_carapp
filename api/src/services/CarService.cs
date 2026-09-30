using data;
using Microsoft.EntityFrameworkCore;
using models;

namespace services;

public class CarService : ICarService
{
    private CarAppContext _context;

    public CarService(CarAppContext context)
    {
        _context = context;
    }

    public core.Paginated<domain.Car> GetCars(domain.CarQuery q)
    {
        IQueryable<Car> query = _context.Cars;

        if (q.Filters?.BrandId != null)
        {
            query = query.Where(c => c.BrandId == q.Filters.BrandId);
        }
        if (q.Filters?.ModelId != null)
        {
            query = query.Where(c => c.ModelId == q.Filters.ModelId);
        }
        if (q.Filters?.Weight != null)
        {
            query = query.Where(c => c.Weight > (q.Filters.Weight.From ?? 0) && c.Weight <= (q.Filters.Weight.To ?? float.PositiveInfinity));
        }
        if (q.Filters?.Year != null)
        {
            query = query.Where(c => c.Year > (q.Filters.Year.From ?? 0) && c.Year <= (q.Filters.Year.To ?? int.MaxValue));
        }

        int total = query.Count();

        bool isDesc = q.SortOrder == core.SortOrder.Desc;

        query = (q.SortBy?.ToLower(), isDesc) switch
        {
            ("brand", false) => query.OrderBy(c => c.Brand.Name),
            ("brand", true) => query.OrderByDescending(c => c.Brand.Name),
            ("year", false) => query.OrderBy(c => c.Year),
            ("year", true) => query.OrderByDescending(c => c.Year),
            ("weight", false) => query.OrderBy(c => c.Weight),
            ("weight", true) => query.OrderByDescending(c => c.Weight),
            (_, false) => query.OrderBy(c => c.Id),
            (_, true) => query.OrderByDescending(c => c.Id)
        };

        query = query.Skip(Math.Max(0, q.PageSize * (q.Page - 1))).Take(q.PageSize);

        var cars = query.ToList();
        return new core.Paginated<domain.Car>
        {
            Items = cars.Select(m => m.ToDomain()).ToList(),
            Total = total,
        };
    }

    public async Task<int> CreateCar(domain.Car car)
    {
        var entry = _context.Cars.Add(Car.FromDomain(car));
        try
        {
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> UpdateCar(domain.Car car)
    {
        if (car.Id == null) return 0;
        _context.Cars.Update(Car.FromDomain(car));
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch
        {
            return 0;
        }
    }
}
