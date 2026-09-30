using domain;

namespace services
{
    public interface ICategoryService
    {
        Category? GetCategory(int id);
        List<Category> GetCategories();
        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category to create.</param>
        /// <returns>The ID of the created category. 0 if creation failed.</returns>
        Task<int> CreateCategory(Category category);
        /// <summary>
        /// Patches an existing category.
        /// </summary>
        /// <param name="category">The full new category to update.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> UpdateCategory(Category category);
        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> DeleteCategory(int id);
    }
}
