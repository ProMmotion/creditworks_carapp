using domain;

namespace managers;

public interface IModelManager
{
    public List<Model> GetModels(int? brandId);
    public Task<core.Result<int>> CreateModel(Model model);
}
