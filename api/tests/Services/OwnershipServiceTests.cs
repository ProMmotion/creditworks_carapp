using data;
using domain;
using services;
using Xunit;
using DomainOwnership = domain.Ownership;

namespace tests;

public class OwnershipServiceTests : IDisposable
{
    private readonly CarAppContext _context = TestDbContext.Create();

    [Fact]
    public async Task CreateOwnership_PersistsAndFiltersByCarIds()
    {
        _context.Brands.Add(new models.Brand { Id = 1, Name = "Honda", ImgUrl = "" });
        _context.Owners.AddRange(
            new models.Owner { Id = 1, Name = "Alex" },
            new models.Owner { Id = 2, Name = "Sam" });
        await _context.SaveChangesAsync();
        _context.Models.Add(new models.Model { Id = 1, Name = "Civic", BrandId = 1 });
        await _context.SaveChangesAsync();
        _context.Cars.AddRange(
            new models.Car { Id = 1, BrandId = 1, ModelId = 1, NumberPlate = "A", Vin = "VIN-A", Year = 2020, Weight = 1400 },
            new models.Car { Id = 2, BrandId = 1, ModelId = 1, NumberPlate = "B", Vin = "VIN-B", Year = 2021, Weight = 1450 });
        await _context.SaveChangesAsync();

        var service = new OwnershipService(_context);
        var id = await service.CreateOwnership(new DomainOwnership { CarId = 1, OwnerId = 1 });
        await service.CreateOwnership(new DomainOwnership { CarId = 2, OwnerId = 2 });

        var matching = Assert.Single(service.GetOwnerships(new[] { 1 }));
        Assert.True(id > 0);
        Assert.Equal(1, matching.CarId);
        Assert.Equal(1, matching.OwnerId);
    }

    public void Dispose() => TestDbContext.Dispose(_context);
}
