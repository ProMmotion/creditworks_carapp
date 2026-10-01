using data;
using Microsoft.EntityFrameworkCore;
using services;
using Xunit;
using DomainModel = domain.Model;

namespace tests;

public class ModelServiceTests : IDisposable
{
    private readonly CarAppContext _context = TestDbContext.Create();

    [Fact]
    public async Task CreateModel_PersistsModelsAndFiltersByBrand()
    {
        _context.Brands.AddRange(
            new models.Brand { Id = 1, Name = "Alpha", ImgUrl = "" },
            new models.Brand { Id = 2, Name = "Beta", ImgUrl = "" });
        await _context.SaveChangesAsync();
        var service = new ModelService(_context);

        var firstId = await service.CreateModel(new DomainModel { BrandId = 1, Name = "A1" });
        await service.CreateModel(new DomainModel { BrandId = 2, Name = "B1" });

        Assert.Equal(2, service.GetModels().Count);
        var brandModel = Assert.Single(service.GetBrandModels(1));
        Assert.Equal(firstId, brandModel.Id);
        Assert.Equal("A1", brandModel.Name);
    }

    public void Dispose() => TestDbContext.Dispose(_context);
}
