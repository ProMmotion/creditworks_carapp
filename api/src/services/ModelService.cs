using data;
using models;

namespace services;

public class ModelService : IModelService
{
    private CarAppContext _context;

    public ModelService(CarAppContext context)
    {
        _context = context;
    }

    public List<domain.Model> GetModels()
    {
        var mModels = _context.Models.ToList();
        return mModels.Select(m => m.ToDomain()).ToList();
    }

    public List<domain.Model> GetBrandModels(int brandId)
    {
        var mModels = _context.Models.Where(m => m.BrandId == brandId).ToList();
        return mModels.Select(m => m.ToDomain()).ToList();
    }

    public async Task<int> CreateModel(domain.Model model)
    {
        var entry = _context.Models.Add(Model.FromDomain(model));
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
}
