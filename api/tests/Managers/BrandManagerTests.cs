using domain;
using managers;
using services;
using Xunit;

namespace tests;

public class BrandManagerTests
{
    [Theory]
    [InlineData(0, false, null)]
    [InlineData(19, true, 19)]
    public async Task CreateBrand_ConvertsServiceIdToResult(int serviceId, bool expectedSuccess, int? expectedId)
    {
        var service = new StubBrandService { CreateResult = serviceId };
        var manager = new BrandManager(service);
        var brand = new Brand { Name = "Example", ImgUrl = "" };

        var result = await manager.CreateBrand(brand);

        Assert.Equal(expectedSuccess, result.IsSuccess);
        if (expectedSuccess) Assert.Equal(expectedId, result.Value);
        else Assert.Equal("Failed to create brand", result.Error);
        Assert.Same(brand, service.ReceivedBrand);
    }

    private sealed class StubBrandService : IBrandService
    {
        public int CreateResult { get; init; }
        public Brand? ReceivedBrand { get; private set; }
        public List<Brand> GetBrands() => new();
        public Task<int> CreateBrand(Brand brand)
        {
            ReceivedBrand = brand;
            return Task.FromResult(CreateResult);
        }
    }
}
