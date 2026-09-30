using domain;
using services;

namespace managers;

public class CategoryManager : ICategoryManager
{
    private readonly ICategoryService _service;

    public CategoryManager(ICategoryService service)
    {
        _service = service;
    }

    public List<Category> GetCategories()
    {
        return _service.GetCategories();
    }

    public async Task<core.Result<int>> CreateCategory(Category category)
    {
        if (!ResolveCategory(category)) return core.Result<int>.Failure("Failed to create category");

        int id = await _service.CreateCategory(category);
        if (id == 0) return core.Result<int>.Failure("Failed to create category");
        return core.Result<int>.Success(id);
    }

    public async Task<core.Result<Category>> PatchCategory(int id, Category category)
    {
        Category? existing = _service.GetCategory(id);
        if (existing == null) return core.Result<Category>.Failure("Category not found");

        existing.Patch(category);
        if (!ResolveCategory(existing)) return core.Result<Category>.Failure("Failed to patch category");

        int updated = await _service.UpdateCategory(existing);
        if (updated == 0) return core.Result<Category>.Failure("Failed to update category");
        return core.Result<Category>.Success(existing);
    }

    public async Task<core.Result<int>> DeleteCategory(int id)
    {
        if (!ResolveCategory(new Category { Id = id, Icon = "", Name = "", Filters = new domain.CarFilter() })) return core.Result<int>.Failure("Failed to delete category");

        int deleted = await _service.DeleteCategory(id);
        if (deleted == 0) return core.Result<int>.Failure("Failed to delete category");
        return core.Result<int>.Success(deleted);
    }

    /// <summary>
    /// Merges all weight categories with the one given to see if ranges are continuous
    /// If param category.id exist in db, param category replaces the one from db for resolving
    /// </summary>
    /// <param name="cat">The category to resolve.</param>
    /// <returns>True if the category was resolved, false otherwise.</returns>
    private bool ResolveCategory(Category cat)
    {
        // select all categories except the one from param
        var weightCategories = _service.GetCategories().Where(x => x.Filters.Weight != null && (cat.Id == null || x.Id != cat.Id)).Select(x => x.Filters.Weight);
        if (weightCategories == null || !weightCategories.Any()) return true; // no weight categories, no need to resolve

        // merge weight categories
        core.Ranges ranges = new core.Ranges(weightCategories);
        if (cat.Filters.Weight != null) ranges.RangesList.Add(cat.Filters.Weight);

        ranges.Merge();

        // check if there's only one range left
        return ranges.RangesList.Count == 1;
    }
}
