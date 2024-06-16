using BLL.Infrastructure.Models.Booking;

namespace BLL.Contracts;
public interface IBookingService
{
    string Add(CreateBookingModel createBookingModel);

    IEnumerable<BookingModel> GetUserBookings(int userId);

    void DeleteExpiredBookings();

    void CancelBooking(int bookingId);
}
