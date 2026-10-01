using data;
using domain;
using services;
using Xunit;

namespace tests;

public class BrandServiceTests : IDisposable
{
    private readonly CarAppContext _context = TestDbContext.Create();

    [Fact]
    public async Task CreateBrand_PersistsBrandAndGetBrandsReturnsIt()
    {
        var service = new BrandService(_context);
        var id = await service.CreateBrand(new Brand { Name = "Honda", ImgUrl = "honda.png" });

        var brand = Assert.Single(service.GetBrands());
        Assert.True(id > 0);
        Assert.Equal(id, brand.Id);
        Assert.Equal("Honda", brand.Name);
        Assert.Equal("honda.png", brand.ImgUrl);
    }

    public void Dispose() => TestDbContext.Dispose(_context);
}
