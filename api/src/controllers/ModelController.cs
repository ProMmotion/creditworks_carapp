using System.Net.Mime;
using managers;
using Microsoft.AspNetCore.Mvc;
using dtos;


namespace controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class ModelsController : ControllerBase
{
    private readonly IModelManager _manager;

    public ModelsController(IModelManager manager)
    {
        _manager = manager;
    }

    [HttpGet("{brandId?}")]
    public IActionResult GetModels(int? brandId = null)
    {
        return Ok(_manager.GetModels(brandId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateModel([FromBody] CreateModelDto dto)
    {
        var result = await _manager.CreateModel(dto.ToDomain());
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
