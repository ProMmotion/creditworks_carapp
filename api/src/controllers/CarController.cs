using System.Net.Mime;
using dtos;
using managers;
using Microsoft.AspNetCore.Mvc;

namespace controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarManager _carManager;

    public CarsController(ICarManager carManager)
    {
        _carManager = carManager;
    }

    [HttpGet]
    public IActionResult GetCars([FromQuery] GetCarDTO query)
    {
        return Ok(_carManager.GetCars(query.ToDomain()));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO car)
    {
        var result = await _carManager.CreateCar(car.ToDomain());
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
