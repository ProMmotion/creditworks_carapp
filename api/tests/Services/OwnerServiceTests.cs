using data;
using domain;
using services;
using Xunit;

namespace tests;

public class OwnerServiceTests : IDisposable
{
    private readonly CarAppContext _context = TestDbContext.Create();

    [Fact]
    public async Task CreateOwner_PersistsOwnerAndGetOwnersReturnsIt()
    {
        var service = new OwnerService(_context);
        var id = await service.CreateOwner(new Owner { Name = "Alex" });

        var owner = Assert.Single(service.GetOwners());
        Assert.True(id > 0);
        Assert.Equal(id, owner.Id);
        Assert.Equal("Alex", owner.Name);
    }

    public void Dispose() => TestDbContext.Dispose(_context);
}
