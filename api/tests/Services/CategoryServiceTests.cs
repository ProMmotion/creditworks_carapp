using data;
using domain;
using services;
using Xunit;
using Range = core.Range;
using DomainCategory = domain.Category;

namespace tests;

public class CategoryServiceTests : IDisposable
{
    private readonly CarAppContext _context = TestDbContext.Create();

    [Fact]
    public async Task CreateUpdateAndDeleteCategory_PersistChanges()
    {
        var service = new CategoryService(_context);
        var id = await service.CreateCategory(new DomainCategory
        {
            Name = "Light",
            Icon = "leaf",
            Filters = new CarFilter { Weight = new Range(0, 1200) },
        });

        var created = Assert.IsType<DomainCategory>(service.GetCategory(id));
        _context.ChangeTracker.Clear();
        Assert.Equal(1200, created.Filters.Weight?.To);
        created.Name = "Updated light";
        created.Filters.Weight = new Range(0, 1300);

        Assert.Equal(1, await service.UpdateCategory(created));
        Assert.Equal("Updated light", service.GetCategory(id)?.Name);
        Assert.Equal(1300, service.GetCategory(id)?.Filters.Weight?.To);

        Assert.Equal(1, await service.DeleteCategory(id));
        Assert.Null(service.GetCategory(id));
        Assert.Equal(0, await service.DeleteCategory(id));
    }

    [Fact]
    public async Task UpdateCategory_ReturnsZeroWhenIdIsMissing()
    {
        var service = new CategoryService(_context);
        var updated = await service.UpdateCategory(new DomainCategory
        {
            Name = "Missing",
            Icon = "",
            Filters = new CarFilter(),
        });

        Assert.Equal(0, updated);
    }

    public void Dispose() => TestDbContext.Dispose(_context);
}
