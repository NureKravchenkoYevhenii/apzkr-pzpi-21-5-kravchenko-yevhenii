using BLL.Contracts;
using BLL.Infrastructure.Models;
using BLL.Infrastructure.Models.Membership;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parky.Controllers;
[Area("memberships")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = $"{nameof(Role.SystemAdmin)},{nameof(Role.ParkingAdmin)}")]
public class MembershipsController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public MembershipsController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] MembershipModel membershipModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _membershipService.Add(membershipModel);

        return Ok();
    }

    [HttpDelete("delete")]
    public IActionResult Delete([FromQuery] int membershipId)
    {
        _membershipService.Delete(membershipId);

        return Ok();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var membershipModels = _membershipService.GetAll();

        return Ok(membershipModels);
    }

    [HttpGet("get")]
    public IActionResult Get([FromQuery] int membershipId)
    {
        var membershipModel = _membershipService.GetById(membershipId);

        return Ok(membershipModel);
    }

    [HttpPost("update")]
    public IActionResult Update([FromBody] MembershipModel membershipModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _membershipService.Update(membershipModel);

        return Ok();
    }

    [HttpPost("purchase")]
    [Authorize]
    public async Task<IActionResult> PurchaseMembership(
        [FromBody] PurchaseMembershipModel purchaseMembershipModel)
    {
        var purchaseString = await _membershipService.PurchaseMembership(
            purchaseMembershipModel);

        return Ok(purchaseString);
    }
}
