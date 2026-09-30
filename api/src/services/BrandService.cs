using data;
using models;

namespace services;

public class BrandService : IBrandService
{
    private CarAppContext _context;

    public BrandService(CarAppContext context)
    {
        _context = context;
    }

    public List<domain.Brand> GetBrands()
    {
        var mBrands = _context.Brands.ToList();
        return mBrands.Select(m => m.ToDomain()).ToList();
    }

    public async Task<int> CreateBrand(domain.Brand brand)
    {
        var entry = _context.Brands.Add(Brand.FromDomain(brand));
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
