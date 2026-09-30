using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using dtos;
using managers;

namespace controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly IBrandManager _brandManager;

    public BrandsController(IBrandManager brandManager)
    {
        _brandManager = brandManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        return Ok(_brandManager.GetBrands());
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDTO brand)
    {
        var result = await _brandManager.CreateBrand(brand.ToDomain());
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
