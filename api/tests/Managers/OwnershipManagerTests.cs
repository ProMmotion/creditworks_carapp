using domain;
using managers;
using services;
using Xunit;

namespace tests;

public class OwnershipManagerTests
{
    [Fact]
    public void GetOwnerships_ForwardsCarIdsToService()
    {
        var service = new StubOwnershipService();
        var manager = new OwnershipManager(service);
        var carIds = new[] { 3, 8 };

        var result = manager.GetOwnerships(carIds);

        Assert.Same(service.Ownerships, result);
        Assert.Same(carIds, service.ReceivedCarIds);
    }

    [Theory]
    [InlineData(0, false, null)]
    [InlineData(17, true, 17)]
    public async Task CreateOwnership_ConvertsServiceIdToResult(int serviceId, bool expectedSuccess, int? expectedId)
    {
        var service = new StubOwnershipService { CreateResult = serviceId };
        var manager = new OwnershipManager(service);
        var ownership = new Ownership { CarId = 3, OwnerId = 6 };

        var result = await manager.CreateOwnership(ownership);

        Assert.Equal(expectedSuccess, result.IsSuccess);
        if (expectedSuccess) Assert.Equal(expectedId, result.Value);
        else Assert.Equal("Failed to create ownership", result.Error);
        Assert.Same(ownership, service.ReceivedOwnership);
    }

    private sealed class StubOwnershipService : IOwnershipService
    {
        public List<Ownership> Ownerships { get; } = new();
        public int[]? ReceivedCarIds { get; private set; }
        public Ownership? ReceivedOwnership { get; private set; }
        public int CreateResult { get; init; }
        public List<Ownership> GetOwnerships(int[] carIds)
        {
            ReceivedCarIds = carIds;
            return Ownerships;
        }
        public Task<int> CreateOwnership(Ownership ownership)
        {
            ReceivedOwnership = ownership;
            return Task.FromResult(CreateResult);
        }
    }
}
