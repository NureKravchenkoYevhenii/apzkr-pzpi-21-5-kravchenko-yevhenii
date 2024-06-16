using BLL.Contracts;
using BLL.Infrastructure.Models.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parky.Controllers;
[Area("cars")]
[Route("api/[area]")]
[ApiController]
[Authorize]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(
        ICarService carService)
    {
        _carService = carService;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] CarModel carModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _carService.Add(carModel);

        return Ok();
    }

    [HttpGet("get-user-cars")]
    public IActionResult GetUserCars([FromQuery] int userId)
    {
        var cars = _carService.GetUserCars(userId);

        return Ok(cars);
    }

    [HttpDelete("delete")]
    public IActionResult Delete([FromQuery] int carId)
    {
        _carService.Delete(carId);

        return Ok();
    }

    [HttpGet("get")]
    public IActionResult Get([FromQuery] int carId)
    {
        var car = _carService.GetById(carId);

        return Ok(car);
    }

    [HttpPost("update")]
    public IActionResult Update([FromBody] CarModel carModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _carService.Update(carModel);

        return Ok();
    }
}
