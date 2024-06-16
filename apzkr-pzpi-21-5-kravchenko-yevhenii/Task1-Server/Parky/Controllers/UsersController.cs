using BLL.Contracts;
using BLL.Infrastructure.Filters;
using BLL.Infrastructure.Models;
using BLL.Infrastructure.Models.Membership;
using Infrastructure.Enums;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parky.Infrastructure.Models;

namespace Parky.Controllers;
[Area("users")]
[Route("api/[area]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IEmailService> _emailService;
    private readonly Lazy<IUserMembershipService> _userMembershipService;

    public UsersController(
        Lazy<IUserService> userService,
        Lazy<IEmailService> emailService,
        Lazy<IUserMembershipService> membershipService)
    {
        _userService = userService;
        _emailService = emailService;
        _userMembershipService = membershipService;
    }

    [HttpPost("register")]
    public ActionResult Register([FromBody] RegisterUserModel registerModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _userService.Value.RegisterUser(registerModel);

        return Ok();
    }

    [HttpPost("update")]
    [Authorize]
    public ActionResult UpdateUserProfile([FromBody] UserProfileInfo userModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(userModel);

        _userService.Value.UpdateUserProfile(userModel);

        return Ok();
    }

    [HttpPost("forgot-password")]
    public ActionResult ForgotPassword([FromBody] ForgotPasswordModel forgotPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _emailService.Value.SendResetPasswordEmail(forgotPassword.Email);

        return Ok();
    }

    [HttpGet("request-reset-password")]
    public ActionResult RequestResetPassword([FromQuery] string token)
    {
        var isTokenValid = _userService.Value.IsResetPasswordTokenValid(token);

        if (isTokenValid)
            return Ok();

        return BadRequest("Invalid reset password token");
    }

    [HttpPost("reset-password")]
    public ActionResult ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _userService.Value.ResetPassword(resetPasswordModel);

        return Ok();
    }

    [HttpGet("get-all")]
    [Authorize(Roles = $"{nameof(Role.ParkingAdmin)},{nameof(Role.SystemAdmin)}")]
    public ActionResult GetAll([FromQuery] UserFilter filter)
    {
        var users = _userService.Value.GetAll(filter);

        return Ok(users);
    }

    [HttpGet("get")]
    [Authorize]
    public ActionResult GetUserProfileById([FromQuery] int userId)
    {
        var user = _userService.Value.GetUserProfileById(userId);

        return Ok(user);
    }

    [HttpPost("set-user-role")]
    [Authorize(Roles = nameof(Role.SystemAdmin))]
    public ActionResult SetUserRole([FromBody] SetUserRoleModel model)
    {
        _userService.Value.SetUserRole(model.UserId, model.Role);

        return Ok();
    }

    [HttpPost("block-membership")]
    [Authorize(Roles = $"{nameof(Role.ParkingAdmin)},{nameof(Role.SystemAdmin)}")]
    public IActionResult BlockMembership([FromBody] BlockMembershipModel blockMembershipModel)
    {
        _userMembershipService.Value.BlockUserMembership(blockMembershipModel);

        return Ok();
    }

    [HttpPost("upload-user-data")]
    [Authorize(Roles = $"{nameof(Role.ParkingAdmin)},{nameof(Role.SystemAdmin)}")]
    public IActionResult UploadUserData([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(Resources.Get("NO_FILE_UPLOADED"));

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest(Resources.Get("INVALID_FILE_FORMAT"));

        using var fileData = file.OpenReadStream();

        _userService.Value.UploadUserData(fileData);

        return Ok();
    }
}
