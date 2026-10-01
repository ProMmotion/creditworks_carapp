using domain;
using managers;
using services;
using Xunit;
using Range = core.Range;

namespace tests;

public class CategoryManagerTests
{
    [Fact]
    public async Task CreateCategory_RejectsDiscontinuousWeightRangesWithoutCallingService()
    {
        var existing = new Category
        {
            Id = 1,
            Name = "Light",
            Icon = "light",
            Filters = new CarFilter { Weight = new Range(0, 1000) },
        };
        var service = new StubCategoryService(existing);
        var manager = new CategoryManager(service);

        var result = await manager.CreateCategory(new Category
        {
            Name = "Heavy",
            Icon = "heavy",
            Filters = new CarFilter { Weight = new Range(1200, 2000) },
        });

        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create category", result.Error);
        Assert.Equal(0, service.CreateCalls);
    }

    [Fact]
    public async Task CreateCategory_AcceptsContinuousWeightRanges()
    {
        var existing = new Category
        {
            Id = 1,
            Name = "Light",
            Icon = "light",
            Filters = new CarFilter { Weight = new Range(0, 1000) },
        };
        var service = new StubCategoryService(existing);
        var manager = new CategoryManager(service);

        var result = await manager.CreateCategory(new Category
        {
            Name = "Heavy",
            Icon = "heavy",
            Filters = new CarFilter { Weight = new Range(1000, 2000) },
        });

        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value);
        Assert.Equal(1, service.CreateCalls);
    }

    [Fact]
    public async Task PatchCategory_ReturnsNotFoundWithoutUpdatingWhenCategoryDoesNotExist()
    {
        var service = new StubCategoryService();
        var manager = new CategoryManager(service);

        var result = await manager.PatchCategory(404, new Category
        {
            Filters = new CarFilter(),
            Icon = "",
            Name = "",
        });

        Assert.False(result.IsSuccess);
        Assert.Equal("Category not found", result.Error);
        Assert.False(service.UpdateCalled);
    }

    private sealed class StubCategoryService(params Category[] categories) : ICategoryService
    {
        public int CreateCalls { get; private set; }
        public bool UpdateCalled { get; private set; }
        public Category? GetCategory(int id) => categories.FirstOrDefault(c => c.Id == id);
        public List<Category> GetCategories() => categories.ToList();
        public Task<int> CreateCategory(Category category)
        {
            CreateCalls++;
            return Task.FromResult(10);
        }
        public Task<int> UpdateCategory(Category category)
        {
            UpdateCalled = true;
            return Task.FromResult(1);
        }
        public Task<int> DeleteCategory(int id) => Task.FromResult(1);
    }
}
