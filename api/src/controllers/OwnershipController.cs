using System.Net.Mime;
using managers;
using Microsoft.AspNetCore.Mvc;
using dtos;

namespace controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class OwnershipsController : ControllerBase
{
    private readonly IOwnershipManager _manager;

    public OwnershipsController(IOwnershipManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetOwnerships([FromQuery] int[] carIds)
    {
        return Ok(_manager.GetOwnerships(carIds));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOwnership([FromBody] CreateOwnershipDto dto)
    {
        var result = await _manager.CreateOwnership(dto.ToDomain());
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
