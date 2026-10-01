using controllers;
using core;
using domain;
using dtos;
using managers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests;

public class CategoriesControllerTests
{
    [Fact]
    public void GetCategories_ReturnsManagerResult()
    {
        var manager = new StubCategoryManager();
        var controller = new CategoriesController(manager);

        var result = Assert.IsType<OkObjectResult>(controller.GetCategories());

        Assert.Same(manager.Categories, result.Value);
    }

    [Fact]
    public async Task CreateCategory_ReturnsOkWithIdAndMapsDtoWhenManagerSucceeds()
    {
        var manager = new StubCategoryManager { CreateResult = Result<int>.Success(8) };
        var controller = new CategoriesController(manager);
        var dto = new CreateCategoryDTO { Name = "Light", Filters = new GetCarFiltersDTO() };

        var result = Assert.IsType<OkObjectResult>(await controller.CreateCategory(dto));

        Assert.Equal(8, result.Value!.GetType().GetProperty("id")!.GetValue(result.Value));
        Assert.Equal("Light", manager.ReceivedCategory?.Name);
    }

    [Fact]
    public async Task CreateCategory_ReturnsBadRequestWhenManagerFails()
    {
        var manager = new StubCategoryManager { CreateResult = Result<int>.Failure("invalid category") };
        var controller = new CategoriesController(manager);

        var result = Assert.IsType<BadRequestObjectResult>(await controller.CreateCategory(
            new CreateCategoryDTO { Name = "Light", Filters = new GetCarFiltersDTO() }));

        Assert.Equal("invalid category", result.Value!.GetType().GetProperty("error")!.GetValue(result.Value));
    }

    [Fact]
    public async Task UpdateCategory_ReturnsUpdatedCategoryWhenManagerSucceeds()
    {
        var updatedCategory = new Category { Id = 8, Name = "Updated", Icon = "", Filters = new CarFilter() };
        var manager = new StubCategoryManager { PatchResult = Result<Category>.Success(updatedCategory) };
        var controller = new CategoriesController(manager);

        var result = Assert.IsType<OkObjectResult>(await controller.UpdateCategory(8, new UpdateCategoryDTO()));

        Assert.Same(updatedCategory, result.Value);
        Assert.Equal(8, manager.ReceivedCategoryId);
    }

    [Fact]
    public async Task UpdateCategory_ReturnsBadRequestWhenManagerFails()
    {
        var manager = new StubCategoryManager { PatchResult = Result<Category>.Failure("category missing") };
        var controller = new CategoriesController(manager);

        var result = Assert.IsType<BadRequestObjectResult>(await controller.UpdateCategory(8, new UpdateCategoryDTO()));

        Assert.Equal("category missing", result.Value!.GetType().GetProperty("error")!.GetValue(result.Value));
    }

    [Fact]
    public async Task DeleteCategory_ReturnsNoContentWhenManagerSucceeds()
    {
        var manager = new StubCategoryManager { DeleteResult = Result<int>.Success(1) };
        var controller = new CategoriesController(manager);

        Assert.IsType<NoContentResult>(await controller.DeleteCategory(5));
    }

    [Fact]
    public async Task DeleteCategory_ReturnsBadRequestWhenManagerFails()
    {
        var manager = new StubCategoryManager { DeleteResult = Result<int>.Failure("not continuous") };
        var controller = new CategoriesController(manager);

        var result = Assert.IsType<BadRequestObjectResult>(await controller.DeleteCategory(5));

        Assert.Equal("not continuous", result.Value!.GetType().GetProperty("error")!.GetValue(result.Value));
    }

    private sealed class StubCategoryManager : ICategoryManager
    {
        public List<Category> Categories { get; } = new();
        public Result<int> CreateResult { get; set; } = Result<int>.Success(1);
        public Result<Category> PatchResult { get; set; } = Result<Category>.Failure("missing");
        public Result<int> DeleteResult { get; set; } = Result<int>.Success(1);
        public Category? ReceivedCategory { get; private set; }
        public int? ReceivedCategoryId { get; private set; }
        public List<Category> GetCategories() => Categories;
        public Task<Result<int>> CreateCategory(Category category)
        {
            ReceivedCategory = category;
            return Task.FromResult(CreateResult);
        }
        public Task<Result<Category>> PatchCategory(int id, Category category)
        {
            ReceivedCategoryId = id;
            ReceivedCategory = category;
            return Task.FromResult(PatchResult);
        }
        public Task<Result<int>> DeleteCategory(int id) => Task.FromResult(DeleteResult);
    }
}
