using domain;
using services;

namespace managers;

public class BrandManager : IBrandManager
{
    private readonly IBrandService _service;
    public BrandManager(IBrandService service)
    {
        _service = service;
    }

    public List<Brand> GetBrands()
    {
        return _service.GetBrands();
    }

    public async Task<core.Result<int>> CreateBrand(Brand brand)
    {
        var id = await _service.CreateBrand(brand);
        if (id == 0) return core.Result<int>.Failure("Failed to create brand");
        return core.Result<int>.Success(id);
    }
}
