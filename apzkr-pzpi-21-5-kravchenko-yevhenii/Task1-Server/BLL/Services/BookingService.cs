using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Booking;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;
public class BookingService : IBookingService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    private readonly Lazy<IRepository<Booking>> _bookings;
    private readonly Lazy<IRepository<User>> _users;
    private readonly Lazy<IRepository<ParkingPlace>> _parkingPlaces;

    public BookingService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _bookings = _unitOfWork.Value.GetLazyRepository<Booking>();
        _users = _unitOfWork.Value.GetLazyRepository<User>();
        _parkingPlaces = _unitOfWork.Value.GetLazyRepository<ParkingPlace>();
    }

    public string Add(CreateBookingModel createBookingModel)
    {
        var user = _users.Value
            .GetAll()
            .Include(u => u.Bookings)
            .FirstOrDefault(u => u.Id == createBookingModel.UserId)
                ?? throw new EntityNotFoundException("USER_NOT_FOUND");

        if (user.Bookings.Count > 0)
            throw new ParkyException("USER_ALREADY_HAS_BOOKING");

        var freeParkingPlace = _parkingPlaces.Value.GetAll()
            .FirstOrDefault(pp => pp.Bookings.Count == 0
                && !pp.ParkingSessions.Where(ps => ps.EndDate == null).Any())
                    ?? throw new ParkyException("NO_FREE_PARKING_PLACES");

        _bookings.Value.Add(new Booking
        {
            BookDate = createBookingModel.BookDate.ToUniversalTime(),
            UserId = user.Id,
            ParkingPlaceId = freeParkingPlace.Id,
            ExpiresIn = TimeSpan.FromMinutes(20)
        });

        return freeParkingPlace.Name;
    }

    public IEnumerable<BookingModel> GetUserBookings(int userId)
    {
        var userBookings = _bookings.Value
            .GetAll()
            .Include(b => b.ParkingPlace)
            .Where(b => b.UserId == userId)
            .ToList();

        var userBookingModels = _mapper.Value.Map<List<BookingModel>>(userBookings);
        
        return userBookingModels;
    }

    public void DeleteExpiredBookings()
    {
        var now = DateTime.UtcNow;
        var expiredBookings = _bookings.Value.GetAll()
            .ToList()
            .Where(b => b.BookDate + b.ExpiresIn <= now);

        foreach (var booking in expiredBookings)
            _bookings.Value.Remove(booking);
    }

    public void CancelBooking(int bookingId)
    {
        var bookingToCancel = _bookings.Value.GetById(bookingId);
        if (bookingToCancel == null)
            return;

        _bookings.Value.Remove(bookingToCancel);
    }
}
