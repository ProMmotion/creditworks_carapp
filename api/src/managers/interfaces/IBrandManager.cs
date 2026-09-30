using domain;

namespace managers;

public interface IBrandManager
{
    public List<Brand> GetBrands();
    public Task<core.Result<int>> CreateBrand(Brand brand);
}
