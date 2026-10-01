using domain;
using managers;
using services;
using Xunit;

namespace tests;

public class ModelManagerTests
{
    [Fact]
    public void GetModels_UsesBrandFilterWhenProvidedAndReturnsAllOtherwise()
    {
        var service = new StubModelService();
        var manager = new ModelManager(service);

        manager.GetModels(4);
        Assert.Equal(4, service.ReceivedBrandId);
        Assert.False(service.GetAllCalled);

        manager.GetModels(null);
        Assert.True(service.GetAllCalled);
    }

    [Theory]
    [InlineData(0, false, null)]
    [InlineData(12, true, 12)]
    public async Task CreateModel_ConvertsServiceIdToResult(int serviceId, bool expectedSuccess, int? expectedId)
    {
        var service = new StubModelService { CreateResult = serviceId };
        var manager = new ModelManager(service);
        var model = new Model { BrandId = 3, Name = "Civic" };

        var result = await manager.CreateModel(model);

        Assert.Equal(expectedSuccess, result.IsSuccess);
        if (expectedSuccess) Assert.Equal(expectedId, result.Value);
        else Assert.Equal("Failed to create model", result.Error);
        Assert.Same(model, service.ReceivedModel);
    }

    private sealed class StubModelService : IModelService
    {
        public int? ReceivedBrandId { get; private set; }
        public bool GetAllCalled { get; private set; }
        public int CreateResult { get; init; }
        public Model? ReceivedModel { get; private set; }
        public List<Model> GetModels()
        {
            GetAllCalled = true;
            return new();
        }
        public List<Model> GetBrandModels(int brandId)
        {
            ReceivedBrandId = brandId;
            return new();
        }
        public Task<int> CreateModel(Model model)
        {
            ReceivedModel = model;
            return Task.FromResult(CreateResult);
        }
    }
}
