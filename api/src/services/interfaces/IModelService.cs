using domain;

namespace services
{
    public interface IModelService
    {
        List<Model> GetModels();
        List<Model> GetBrandModels(int brandId);
        /// <summary>
        /// Creates a new model.
        /// </summary>
        /// <param name="model">The model to create.</param>
        /// <returns>The ID of the created model. 0 if creation failed</returns>
        Task<int> CreateModel(Model model);
    }
}
