using data;
using models;

namespace services;

public class CategoryService : ICategoryService
{
    private readonly CarAppContext _context;

    public CategoryService(CarAppContext context)
    {
        _context = context;
    }

    public domain.Category? GetCategory(int id)
    {
        return _context.Categories.Find(id)?.ToDomain();
    }

    public List<domain.Category> GetCategories()
    {
        var categories = _context.Categories.ToList();
        return categories.Select(m => m.ToDomain()).ToList();
    }

    public async Task<int> CreateCategory(domain.Category category)
    {
        var entry = _context.Categories.Add(Category.FromDomain(category));
        try
        {
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> UpdateCategory(domain.Category category)
    {
        if (category.Id == null) return 0;
        _context.Categories.Update(Category.FromDomain(category));
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> DeleteCategory(int id)
    {
        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null) return 0;

        _context.Categories.Remove(existingCategory);
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch
        {
            return 0;
        }
    }
}
