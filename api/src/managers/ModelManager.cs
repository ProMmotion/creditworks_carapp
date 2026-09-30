using domain;
using services;

namespace managers;

public class ModelManager : IModelManager
{
    private readonly IModelService _service;
    public ModelManager(IModelService service)
    {
        _service = service;
    }

    public List<Model> GetModels(int? brandId)
    {
        if (brandId != null) return _service.GetBrandModels(brandId.Value);
        return _service.GetModels();
    }

    public async Task<core.Result<int>> CreateModel(Model model)
    {
        int id = await _service.CreateModel(model);
        if (id == 0) return core.Result<int>.Failure("Failed to create model");
        return core.Result<int>.Success(id);
    }
}
