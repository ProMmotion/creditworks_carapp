using domain;

namespace services;

public interface IBrandService
{
    List<Brand> GetBrands();
    /// <summary>
    /// Creates a new brand.
    /// </summary>
    /// <param name="brand">The brand to create.</param>
    /// <returns>The ID of the created brand. 0 if creation failed.</returns>
    Task<int> CreateBrand(Brand brand);
}
