using controllers;
using core;
using domain;
using dtos;
using managers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests;

public class OwnershipsControllerTests
{
    [Fact]
    public void GetOwnerships_ReturnsResultsAndPassesCarIdsToManager()
    {
        var manager = new StubOwnershipManager();
        var controller = new OwnershipsController(manager);
        var carIds = new[] { 2, 4 };

        var result = Assert.IsType<OkObjectResult>(controller.GetOwnerships(carIds));

        Assert.Same(manager.Ownerships, result.Value);
        Assert.Equal(carIds, manager.ReceivedCarIds);
    }

    [Fact]
    public async Task CreateOwnership_ReturnsOkWithIdAndMapsDtoWhenManagerSucceeds()
    {
        var manager = new StubOwnershipManager { CreateResult = Result<int>.Success(6) };
        var controller = new OwnershipsController(manager);
        var dto = new CreateOwnershipDto { CarId = 4, OwnerId = 7 };

        var result = Assert.IsType<OkObjectResult>(await controller.CreateOwnership(dto));

        Assert.Equal(6, result.Value!.GetType().GetProperty("id")!.GetValue(result.Value));
        Assert.Equal(4, manager.ReceivedOwnership?.CarId);
        Assert.Equal(7, manager.ReceivedOwnership?.OwnerId);
    }

    [Fact]
    public async Task CreateOwnership_ReturnsBadRequestWhenManagerFails()
    {
        var manager = new StubOwnershipManager { CreateResult = Result<int>.Failure("ownership rejected") };
        var controller = new OwnershipsController(manager);

        var result = Assert.IsType<BadRequestObjectResult>(await controller.CreateOwnership(new CreateOwnershipDto { CarId = 4, OwnerId = 7 }));

        Assert.Equal("ownership rejected", result.Value!.GetType().GetProperty("error")!.GetValue(result.Value));
    }

    private sealed class StubOwnershipManager : IOwnershipManager
    {
        public List<Ownership> Ownerships { get; } = new();
        public int[]? ReceivedCarIds { get; private set; }
        public Ownership? ReceivedOwnership { get; private set; }
        public Result<int> CreateResult { get; set; } = Result<int>.Success(1);
        public List<Ownership> GetOwnerships(int[] carIds)
        {
            ReceivedCarIds = carIds;
            return Ownerships;
        }
        public Task<Result<int>> CreateOwnership(Ownership ownership)
        {
            ReceivedOwnership = ownership;
            return Task.FromResult(CreateResult);
        }
    }
}
