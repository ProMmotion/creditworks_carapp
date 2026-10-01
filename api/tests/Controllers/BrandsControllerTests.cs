using controllers;
using core;
using domain;
using dtos;
using managers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests;

public class BrandsControllerTests
{
    [Fact]
    public async Task GetBrands_ReturnsManagerResult()
    {
        var manager = new StubBrandManager();
        var controller = new BrandsController(manager);

        var result = Assert.IsType<OkObjectResult>(await controller.GetBrands());

        Assert.Same(manager.Brands, result.Value);
    }

    [Fact]
    public async Task CreateBrand_ReturnsOkWithIdAndMapsDtoWhenManagerSucceeds()
    {
        var manager = new StubBrandManager { CreateResult = Result<int>.Success(31) };
        var controller = new BrandsController(manager);

        var result = Assert.IsType<OkObjectResult>(await controller.CreateBrand(new CreateBrandDTO { Name = "Honda" }));

        Assert.Equal(31, result.Value!.GetType().GetProperty("id")!.GetValue(result.Value));
        Assert.Equal("Honda", manager.ReceivedBrand?.Name);
    }

    [Fact]
    public async Task CreateBrand_ReturnsBadRequestWhenManagerFails()
    {
        var manager = new StubBrandManager { CreateResult = Result<int>.Failure("brand rejected") };
        var controller = new BrandsController(manager);

        var result = Assert.IsType<BadRequestObjectResult>(await controller.CreateBrand(new CreateBrandDTO { Name = "Honda" }));

        Assert.Equal("brand rejected", result.Value!.GetType().GetProperty("error")!.GetValue(result.Value));
    }

    private sealed class StubBrandManager : IBrandManager
    {
        public List<Brand> Brands { get; } = new();
        public Result<int> CreateResult { get; set; } = Result<int>.Success(1);
        public Brand? ReceivedBrand { get; private set; }
        public List<Brand> GetBrands() => Brands;
        public Task<Result<int>> CreateBrand(Brand brand)
        {
            ReceivedBrand = brand;
            return Task.FromResult(CreateResult);
        }
    }
}
