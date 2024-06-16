using BLL.Contracts;
using BLL.Infrastructure.Models.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parky.Controllers;
[Area("bookings")]
[Route("api/[area]")]
[ApiController]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("make-booking")]
    public IActionResult MakeBooking([FromBody] CreateBookingModel createBookingModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _bookingService.Add(createBookingModel);

        return Ok();
    }

    [HttpDelete("cancel-booking")]
    public IActionResult CancelBooking([FromQuery] int bookingId)
    {
        _bookingService.CancelBooking(bookingId);

        return Ok();
    }

    [HttpGet("get-user-bookings")]
    public IActionResult GetUserBookings([FromQuery] int userId)
    {
        var userBookings = _bookingService.GetUserBookings(userId);

        return Ok(userBookings);
    }
}
