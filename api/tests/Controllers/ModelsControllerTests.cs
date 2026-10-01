using controllers;
using core;
using domain;
using dtos;
using managers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests;

public class ModelsControllerTests
{
    [Fact]
    public void GetModels_ReturnsManagerResultsAndPassesOptionalBrandId()
    {
        var manager = new StubModelManager();
        var controller = new ModelsController(manager);

        var filteredResult = Assert.IsType<OkObjectResult>(controller.GetModels(12));
        Assert.Same(manager.Models, filteredResult.Value);
        Assert.Equal(12, manager.ReceivedBrandId);

        var allResult = Assert.IsType<OkObjectResult>(controller.GetModels());
        Assert.Same(manager.Models, allResult.Value);
        Assert.Null(manager.ReceivedBrandId);
    }

    [Fact]
    public async Task CreateModel_ReturnsOkWithIdAndMapsDtoWhenManagerSucceeds()
    {
        var manager = new StubModelManager { CreateResult = Result<int>.Success(14) };
        var controller = new ModelsController(manager);
        var dto = new CreateModelDto { BrandId = 3, Name = "Civic" };

        var result = Assert.IsType<OkObjectResult>(await controller.CreateModel(dto));

        Assert.Equal(14, result.Value!.GetType().GetProperty("id")!.GetValue(result.Value));
        Assert.Equal(3, manager.ReceivedModel?.BrandId);
        Assert.Equal("Civic", manager.ReceivedModel?.Name);
    }

    [Fact]
    public async Task CreateModel_ReturnsBadRequestWhenManagerFails()
    {
        var manager = new StubModelManager { CreateResult = Result<int>.Failure("model rejected") };
        var controller = new ModelsController(manager);

        var result = Assert.IsType<BadRequestObjectResult>(await controller.CreateModel(new CreateModelDto { BrandId = 3, Name = "Civic" }));

        Assert.Equal("model rejected", result.Value!.GetType().GetProperty("error")!.GetValue(result.Value));
    }

    private sealed class StubModelManager : IModelManager
    {
        public List<Model> Models { get; } = new();
        public int? ReceivedBrandId { get; private set; }
        public Result<int> CreateResult { get; set; } = Result<int>.Success(1);
        public Model? ReceivedModel { get; private set; }
        public List<Model> GetModels(int? brandId)
        {
            ReceivedBrandId = brandId;
            return Models;
        }
        public Task<Result<int>> CreateModel(Model model)
        {
            ReceivedModel = model;
            return Task.FromResult(CreateResult);
        }
    }
}
