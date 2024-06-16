using BLL.Contracts;
using Infrastructure.Enums;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parky.Controllers;
[Area("parking-sessions")]
[Route("api/[area]")]
[ApiController]
[Authorize]
public class ParkingSessionsController : ControllerBase
{
    private readonly IParkingSessionService _parkingSessionService;

    public ParkingSessionsController(IParkingSessionService parkingSessionService)
    {
        _parkingSessionService = parkingSessionService;
    }

    [HttpPost("check-in")]
    public IActionResult CheckIn([FromBody] string carNumber)
    {
        var result = _parkingSessionService.CheckIn(carNumber);

        return Ok(result);
    }

    [HttpPost("check-out")]
    public IActionResult CheckOut([FromBody] string carNumber)
    {
        var result = _parkingSessionService.CheckOut(carNumber);

        return Ok(result);
    }

    [HttpGet("user-parking-sessions")]
    public IActionResult GetUserParkingSessions([FromQuery] int userId)
    {
        var userParkingSessions = _parkingSessionService.GetUserParkingSessions(userId);

        return Ok(userParkingSessions);
    }

    [HttpGet("get-parking-statistics")]
    [Authorize(Roles = $"{nameof(Role.ParkingAdmin)},{nameof(Role.SystemAdmin)}")]
    public IActionResult GetParkingStatistics(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        var occupancyStatistics = _parkingSessionService.GetParkingStatistics(from, to);

        return File(
            occupancyStatistics,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            string.Format(Resources.Get("PARKING_STATISTICS_FILE_NAME"), from.ToShortDateString(), to.ToShortDateString()));
    }
}
