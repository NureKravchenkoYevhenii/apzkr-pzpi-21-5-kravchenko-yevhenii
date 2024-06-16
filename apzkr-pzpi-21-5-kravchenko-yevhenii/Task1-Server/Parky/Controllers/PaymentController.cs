using BLL.Contracts;
using BLL.Infrastructure.Models.Payment;
using Infrastructure.Enums;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parky.Controllers;
[Area("payments")]
[Route("api/[area]")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("get")]
    public IActionResult GetById([FromQuery] int paymentId)
    {
        var payment = _paymentService.GetById(paymentId);

        return Ok(payment);
    }

    [HttpGet("get-user-payments")]
    public IActionResult GetUserPayments([FromQuery] int userId)
    {
        var userPayments = _paymentService.GetUserPayments(userId);

        return Ok(userPayments);
    }

    [HttpPost("make-payment")]
    public async Task<IActionResult> MakePayment([FromBody] int paymentId)
    {
        var paymentUrl = await _paymentService.MakePayment(paymentId);

        return Ok(paymentUrl);
    }

    [HttpPost("payment-result")]
    [AllowAnonymous]
    public IActionResult GetPaymentResult([FromForm] PaymentApiResponse paymentResponse)
    {
        _paymentService.ProcessPayment(paymentResponse);
             
        return Ok();
    }

    [HttpGet("get-statistics")]
    [Authorize(Roles = $"{nameof(Role.ParkingAdmin)},{nameof(Role.SystemAdmin)}")]
    public IActionResult GetPaymentStatistics(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        var paymentStatistics = _paymentService.GetPaymentStatistics(from, to);

        return File(
            paymentStatistics,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            string.Format(Resources.Get("PAYMENT_STATISTICS_FILE_NAME"), from.ToShortDateString(), to.ToShortDateString()));
    }
}
