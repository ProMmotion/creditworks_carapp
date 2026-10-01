using controllers;
using core;
using domain;
using dtos;
using managers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests;

public class OwnersControllerTests
{
    [Fact]
    public void GetOwners_ReturnsManagerResult()
    {
        var manager = new StubOwnerManager();
        var controller = new OwnersController(manager);

        var result = Assert.IsType<OkObjectResult>(controller.GetOwners());

        Assert.Same(manager.Owners, result.Value);
    }

    [Fact]
    public async Task CreateOwner_ReturnsOkWithIdAndMapsDtoWhenManagerSucceeds()
    {
        var manager = new StubOwnerManager { CreateResult = Result<int>.Success(5) };
        var controller = new OwnersController(manager);
        var dto = new CreateOwnerDto { Name = "Alex" };

        var result = Assert.IsType<OkObjectResult>(await controller.CreateOwner(dto));

        Assert.Equal(5, result.Value!.GetType().GetProperty("id")!.GetValue(result.Value));
        Assert.Equal("Alex", manager.ReceivedOwner?.Name);
    }

    [Fact]
    public async Task CreateOwner_ReturnsBadRequestWhenManagerFails()
    {
        var manager = new StubOwnerManager { CreateResult = Result<int>.Failure("owner rejected") };
        var controller = new OwnersController(manager);

        var result = Assert.IsType<BadRequestObjectResult>(await controller.CreateOwner(new CreateOwnerDto { Name = "Alex" }));

        Assert.Equal("owner rejected", result.Value!.GetType().GetProperty("error")!.GetValue(result.Value));
    }

    private sealed class StubOwnerManager : IOwnerManager
    {
        public List<Owner> Owners { get; } = new();
        public Result<int> CreateResult { get; set; } = Result<int>.Success(1);
        public Owner? ReceivedOwner { get; private set; }
        public List<Owner> GetOwners() => Owners;
        public Task<Result<int>> CreateOwner(Owner owner)
        {
            ReceivedOwner = owner;
            return Task.FromResult(CreateResult);
        }
    }
}
