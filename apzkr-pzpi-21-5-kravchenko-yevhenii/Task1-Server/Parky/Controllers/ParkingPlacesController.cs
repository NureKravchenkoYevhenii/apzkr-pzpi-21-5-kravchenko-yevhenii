using BLL.Contracts;
using BLL.Infrastructure.Models.ParkingPlace;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parky.Controllers;
[Area("parking-places")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = $"{nameof(Role.SystemAdmin)},{nameof(Role.ParkingAdmin)}")]
public class ParkingPlacesController : ControllerBase
{
    private readonly IParkingPlaceService _parkingPlaceService;

    public ParkingPlacesController(IParkingPlaceService parkingPlaceService)
    {
        _parkingPlaceService = parkingPlaceService;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] ParkingPlaceModel parkingPlaceModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _parkingPlaceService.Add(parkingPlaceModel);

        return Ok();
    }

    [HttpDelete("delete")]
    public IActionResult Delete([FromQuery] int parkingPlaceId)
    {
        _parkingPlaceService.Delete(parkingPlaceId);

        return Ok();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var parkingPlaces = _parkingPlaceService.GetAll().ToList();

        return Ok(parkingPlaces);
    }

    [HttpGet("get")]
    public IActionResult GetById([FromQuery] int parkingPlaceId)
    {
        var parkingPlace = _parkingPlaceService.GetById(parkingPlaceId);

        return Ok(parkingPlace);
    }

    [HttpPost("update")]
    public IActionResult Update([FromBody] ParkingPlaceModel parkingPlaceModel)
    {
        _parkingPlaceService.Update(parkingPlaceModel);

        return Ok();
    }

    [HttpGet("occupancy-statistics")]
    [Authorize]
    public IActionResult GetOccupancyStatistics()
    {
        var statistics = _parkingPlaceService.GetOccupancyStatistics();

        return Ok(statistics);
    }
}
