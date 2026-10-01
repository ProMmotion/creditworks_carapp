using data;
using Microsoft.EntityFrameworkCore;

namespace tests;

internal static class TestDbContext
{
    public static CarAppContext Create()
    {
        var options = new DbContextOptionsBuilder<CarAppContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        var context = new CarAppContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    public static void Dispose(CarAppContext context)
    {
        context.Database.CloseConnection();
        context.Dispose();
    }
}
