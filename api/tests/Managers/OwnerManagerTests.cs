using domain;
using managers;
using services;
using Xunit;

namespace tests;

public class OwnerManagerTests
{
    [Theory]
    [InlineData(0, false, null)]
    [InlineData(17, true, 17)]
    public async Task CreateOwner_ConvertsServiceIdToResult(int serviceId, bool expectedSuccess, int? expectedId)
    {
        var service = new StubOwnerService { CreateResult = serviceId };
        var manager = new OwnerManager(service);
        var owner = new Owner { Name = "Alex" };

        var result = await manager.CreateOwner(owner);

        Assert.Equal(expectedSuccess, result.IsSuccess);
        if (expectedSuccess) Assert.Equal(expectedId, result.Value);
        else Assert.Equal("Failed to create owner", result.Error);
        Assert.Same(owner, service.ReceivedOwner);
    }

    private sealed class StubOwnerService : IOwnerService
    {
        public int CreateResult { get; init; }
        public Owner? ReceivedOwner { get; private set; }
        public List<Owner> GetOwners() => new();
        public Task<int> CreateOwner(Owner owner)
        {
            ReceivedOwner = owner;
            return Task.FromResult(CreateResult);
        }
    }
}
