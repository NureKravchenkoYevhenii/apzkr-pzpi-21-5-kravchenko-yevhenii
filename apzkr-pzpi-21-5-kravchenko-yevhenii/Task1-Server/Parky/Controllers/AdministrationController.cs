using BLL.Contracts;
using BLL.Infrastructure.Models.ParkingSettings;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parky.Infrastructure.Models;

namespace Parky.Controllers;
[Area("administrations")]
[Route("api/[area]")]
[ApiController]
public class AdministrationController : ControllerBase
{
    private readonly IAdministrationService _administrationService;

    public AdministrationController(
        IAdministrationService administrationService)
    {
        _administrationService = administrationService;
    }

    [HttpPost("backup-database")]
    [Authorize(Roles = nameof(Role.SystemAdmin))]
    public IActionResult BackupDatabase([FromBody] SavePath savePath)
    {
        _administrationService.BackupDatabase(savePath.SavePathStr);

        return Ok();
    }

    [HttpPost("update-parking-settings")]
    [Authorize(Roles = $"{nameof(Role.ParkingAdmin)},{nameof(Role.SystemAdmin)}")]
    public IActionResult UpdateParkingSettings([FromBody] ParkingSettingsModel parkingSettingsModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _administrationService.UpdateParkingSettings(parkingSettingsModel);

        return Ok();
    }

    [HttpGet("get-parking-settings")]
    [Authorize(Roles = $"{nameof(Role.ParkingAdmin)},{nameof(Role.SystemAdmin)}")]
    public IActionResult GetParkingSettings()
    {
        var parkingSettings = _administrationService.GetParkingSettings();

        return Ok(parkingSettings);
    }
}
