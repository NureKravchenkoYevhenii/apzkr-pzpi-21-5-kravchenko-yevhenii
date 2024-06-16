using BLL.Contracts;
using BLL.Infrastructure.Models.Tarif;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parky.Controllers;
[Area("tarifs")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = $"{nameof(Role.ParkingAdmin)},{nameof(Role.SystemAdmin)}")]
public class TarifsController : ControllerBase
{
    private readonly ITarifService _tarifService;

    public TarifsController(ITarifService tarifService)
    {
        _tarifService = tarifService;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] TarifModel tarifModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _tarifService.Add(tarifModel);

        return Ok();
    }

    [HttpDelete("delete")]
    public IActionResult Delete([FromQuery] int tarifId)
    {
        _tarifService.Delete(tarifId);

        return Ok();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var tarifs = _tarifService.GetAll();

        return Ok(tarifs);
    }

    [HttpGet("get")]
    public IActionResult GetById([FromQuery] int tarifId)
    {
        var tarif = _tarifService.GetById(tarifId);

        return Ok(tarif);
    }

    [HttpPost("update")]
    public IActionResult Update([FromBody] TarifModel tarifModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _tarifService.Update(tarifModel);

        return Ok();
    }
}
