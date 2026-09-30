using System.Net.Mime;
using managers;
using Microsoft.AspNetCore.Mvc;
using dtos;

namespace controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly IOwnerManager _manager;

    public OwnersController(IOwnerManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetOwners()
    {
        return Ok(_manager.GetOwners());
    }

    [HttpPost]
    public async Task<IActionResult> CreateOwner([FromBody] CreateOwnerDto dto)
    {
        var result = await _manager.CreateOwner(dto.ToDomain());
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
