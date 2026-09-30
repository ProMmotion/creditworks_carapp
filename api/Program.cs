using Microsoft.EntityFrameworkCore;
using data;
using services;
using managers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddScoped<ICarManager, CarManager>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICategoryManager, CategoryManager>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandManager, BrandManager>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IModelManager, ModelManager>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IOwnerManager, OwnerManager>();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IOwnershipManager, OwnershipManager>();
builder.Services.AddScoped<IOwnershipService, OwnershipService>();

var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
var DbPath = System.IO.Path.Join(path, "carapp.db");

builder.Services.AddDbContext<CarAppContext>((opt) => opt.UseSqlite($"Data Source={DbPath}"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CarAppContext>();
        await DbInitializer.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Une erreur est survenue lors du seeding de la base de données.");
    }
}

// app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();
