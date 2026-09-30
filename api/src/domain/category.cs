namespace domain;

public class Category
{
    public int? Id { get; set; }
    public CarFilter Filters { get; set; }
    public string Icon { get; set; }
    public string Name { get; set; }

    public void Patch(Category cat)
    {
        if (cat.Filters.BrandId != Filters.BrandId) Filters.BrandId = cat.Filters.BrandId;
        if (cat.Filters.ModelId != Filters.ModelId) Filters.ModelId = cat.Filters.ModelId;
        if (Filters.Weight == null || (cat.Filters.Weight != null && !Filters.Weight.Equals(cat.Filters.Weight))) Filters.Weight = cat.Filters.Weight;
        if (Filters.Year == null || (cat.Filters.Year != null && !Filters.Year.Equals(cat.Filters.Year))) Filters.Year = cat.Filters.Year;
        if (cat.Icon != "") Icon = cat.Icon;
        if (cat.Name != "") Name = cat.Name;
    }
}
