using domain;

namespace managers;

public interface ICategoryManager
{
    public List<Category> GetCategories();
    public Task<core.Result<int>> CreateCategory(Category category);
    public Task<core.Result<Category>> PatchCategory(int id, Category category);
    public Task<core.Result<int>> DeleteCategory(int id);
}
