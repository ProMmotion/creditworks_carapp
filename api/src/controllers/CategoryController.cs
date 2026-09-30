using System.Net.Mime;
using dtos;
using managers;
using Microsoft.AspNetCore.Mvc;

namespace controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryManager _manager;

    public CategoriesController(ICategoryManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetCategories()
    {
        return Ok(_manager.GetCategories());
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO category)
    {
        var result = await _manager.CreateCategory(category.ToDomain());
        if (result.IsSuccess)
        {
            return Ok(new { id = result.Value });
        }
        return BadRequest(new { error = result.Error });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO category)
    {
        var result = await _manager.PatchCategory(id, category.ToDomain());
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(new { error = result.Error });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _manager.DeleteCategory(id);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return BadRequest(new { error = result.Error });
    }
}
