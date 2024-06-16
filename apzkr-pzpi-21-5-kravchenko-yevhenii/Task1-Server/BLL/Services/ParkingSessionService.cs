using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.ParkingSession;
using BLL.Infrastructure.Models.Payment;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Enums;
using Infrastructure.Exceptions;
using Infrastructure.Resources;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace BLL.Services;
public class ParkingSessionService : IParkingSessionService
{
    private const int HOURES_IN_DAY = 24;
    private const int MINUTES_IN_HOUR = 60;

    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;
    private readonly Lazy<IPaymentService> _paymentService;

    private readonly Lazy<IRepository<ParkingSession>> _parkingSessions;
    private readonly Lazy<IRepository<Car>> _cars;
    private readonly Lazy<IRepository<ParkingPlace>> _parkingPlaces;
    private readonly Lazy<IRepository<Tarif>> _tarifs;

    public ParkingSessionService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper,
        Lazy<IPaymentService> paymentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paymentService = paymentService;

        _parkingSessions = _unitOfWork.Value.GetLazyRepository<ParkingSession>();
        _cars = _unitOfWork.Value.GetLazyRepository<Car>();
        _parkingPlaces = _unitOfWork.Value.GetLazyRepository<ParkingPlace>();
        _tarifs = _unitOfWork.Value.GetLazyRepository<Tarif>();
    }

    public bool CheckIn(string carNumber)
    {
        var car = _cars.Value
            .GetAll()
            .Include(c => c.User)
            .ThenInclude(u => u.Bookings)
            .FirstOrDefault(c => c.CarNumber == carNumber);

        if (car == null)
            return false;

        int parkingPlaceId = car.User.Bookings.FirstOrDefault()?.ParkingPlaceId 
            ?? _parkingPlaces.Value
                    .Get(pp => pp.Bookings.Count == 0
                        && pp.ParkingSessions.All(ps => ps.EndDate != null))!.Id;

        _parkingSessions.Value.Add(new ParkingSession
        {
            StartDate = DateTime.UtcNow,
            UserId = car.User.Id,
            ParkingPlaceId = parkingPlaceId
        });

        return true;
    }

    public bool CheckOut(string carNumber)
    {
        using var scope = _unitOfWork.Value.BeginTransaction();
        try
        {
            var result = new CheckOutResult();
            var car = _cars.Value
                .GetAll()
                .Include(c => c.User)
                .ThenInclude(u => u.UserMemberships)
                .ThenInclude(um => um.Membership)
                .FirstOrDefault(c => c.CarNumber == carNumber)
                    ?? throw new ParkyException("CAR_NOT_FOUND");

            var parkingSession = _parkingSessions.Value.Get(ps => ps.UserId == car.UserId && ps.EndDate == null)
                ?? throw new ParkyException("PARKING_SESSION_NOT_FOUND");

            parkingSession.EndDate = DateTime.UtcNow;

            if (!car.User.UserMemberships.Any(um => um.StartDate.Add(TimeSpan.FromDays(um.Membership.DurationInDays)) > DateTime.UtcNow))
            {

                var paymentModel = new PaymentModel
                {
                    Id = 0,
                    Transaction = string.Empty,
                    Sum = GetParkingSessionPrice(parkingSession),
                    Description = string.Empty,
                    PurchaseType = PurchaseType.PayForParkingSession,
                    UserId = parkingSession.UserId,
                };

                _paymentService.Value.Add(paymentModel);
            }
            
            _parkingSessions.Value.Update(parkingSession);

            scope.Commit();
        }
        catch (Exception)
        {
            scope.Rollback();
            return false;
        }
        
        return true;
    }

    public IEnumerable<ParkingSessionModel> GetUserParkingSessions(int userId)
    {
        var userParkingSessions = _parkingSessions.Value
            .GetAll()
            .Include(ps => ps.ParkingPlace)
            .Where(ps => ps.UserId == userId)
            .ToList();

        var userParkingSessionModels = _mapper.Value
            .Map<List<ParkingSessionModel>>(userParkingSessions);

        return userParkingSessionModels;
    }

    public byte[] GetParkingStatistics(DateTime from, DateTime to)
    {
        from = from.ToUniversalTime();
        to = to.ToUniversalTime();

        var parkingSessions = _parkingSessions.Value
            .GetAll()
            .Where(ps => ps.EndDate.HasValue
                && ps.StartDate >= from && ps.EndDate.Value <= to)
            .ToList();

        var occupancyStatistics = new List<OccupancyStatisticsRow>();
        var currentDate = from.Date;

        while (currentDate <= to)
        {
            for (int hour = 0; hour < 24; hour++)
            {
                var hourStart = currentDate.AddHours(hour);
                var hourEnd = hourStart.AddHours(1);

                var occupiedPlacesAmount = parkingSessions.Count(p =>
                    (p.StartDate <= hourEnd && p.EndDate >= hourStart) 
                    || (p.StartDate <= hourStart && p.EndDate >= hourEnd)
                    || (p.StartDate >= hourStart && p.EndDate <= hourEnd));

                occupancyStatistics.Add(new OccupancyStatisticsRow
                {
                    Date = currentDate,
                    Hour = hour,
                    OccupiedPlacesAmount = occupiedPlacesAmount
                });
            }

            currentDate = currentDate.AddDays(1);
        }

        using var excelPackage = new ExcelPackage();
        using var worksheet = excelPackage.Workbook.Worksheets.Add(
            Resources.Get("PARKING_OCCUPANCY_STATISTICS"));

        worksheet.Cells["A1"].Value = Resources.Get("DATE");
        worksheet.Cells["B1"].Value = Resources.Get("HOUR");
        worksheet.Cells["C1"].Value = Resources.Get("OCCUPIED_PLACES_AMOUNT");

        var row = 2;
        foreach (var stat in occupancyStatistics)
        {
            worksheet.Cells[$"A{row}"].Value = stat.Date.ToString("yyyy-MM-dd");
            worksheet.Cells[$"B{row}"].Value = stat.Hour;
            worksheet.Cells[$"C{row}"].Value = stat.OccupiedPlacesAmount;
            row++;
        }

        var chart = worksheet.Drawings.AddChart(
            Resources.Get("OCCUPANCY_CHART"),
            eChartType.Line);
        var series = chart.Series.Add(
            worksheet.Cells["C2:C" + row],
            worksheet.Cells["B2:B" + row]);

        series.Header = Resources.Get("OCCUPIED_PLACES");
        chart.SetPosition(5, 0, 5, 0);
        chart.SetSize(600, 400);

        var excelBytes = excelPackage.GetAsByteArray();

        return excelBytes;
    }

    private decimal GetParkingSessionPrice(ParkingSession parkingSession)
    {
        var tarifs = _tarifs.Value.GetAll().ToList();
        var price = 0M;

        if (tarifs.Count == 0)
            return price;

        var parkingSessionTimePerDay = GetParkingSessionTimePerDay(parkingSession);

        foreach (var tariff in tarifs)
        {
            price += CalculateSessionPriceInTariffHours(
                parkingSessionTimePerDay,
                tariff);
        }

        return Math.Round(price, 2);
    }

    private Dictionary<DayOfWeek, List<Tuple<TimeSpan, TimeSpan>>> GetParkingSessionTimePerDay(
        ParkingSession parkingSession)
    {
        var sessionTimePerDay = new Dictionary<DayOfWeek, List<Tuple<TimeSpan, TimeSpan>>>();

        var startDate = parkingSession.StartDate.Date;
        var endDate = parkingSession.EndDate!.Value.Date;

        for (var currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
        {
            var startTime = parkingSession.StartDate.Date == currentDate
                ? parkingSession.StartDate.TimeOfDay
                : TimeSpan.Zero;

            var endTime = parkingSession.EndDate!.Value.Date == currentDate
                ? parkingSession.EndDate.Value.TimeOfDay
                : TimeSpan.FromDays(1);

            if (sessionTimePerDay.TryGetValue(currentDate.DayOfWeek, out var value))
            {
                value.Add(new Tuple<TimeSpan, TimeSpan>(
                    startTime,
                    endTime));
            }
            else
            {
                sessionTimePerDay[currentDate.DayOfWeek] = new List<Tuple<TimeSpan, TimeSpan>>
                {
                    new Tuple<TimeSpan, TimeSpan>(
                        startTime,
                        endTime)
                };
            }
        }

        return sessionTimePerDay;
    }

    private decimal CalculateSessionPriceInTariffHours(
        Dictionary<DayOfWeek, List<Tuple<TimeSpan, TimeSpan>>> parkingSessionTimePerDay,
        Tarif tariff)
    {
        var activeDaysOfWeekForTarif = tariff.ActiveOnDaysOfWeek.Split(",")
            .Select(x => (DayOfWeek)Convert.ToInt32(x))
            .ToList();

        var tariffStartTime = tariff.StartTime.ToTimeSpan();
        var tariffEndTime = tariff.EndTime.ToTimeSpan();
        var tariffPricePerMinute = GetTariffPricePerMinute(tariff);

        var price = 0M;
        foreach (var key in parkingSessionTimePerDay.Keys)
        {
            if (!activeDaysOfWeekForTarif.Contains(key))
                continue;

            var sessionTimePerDay = parkingSessionTimePerDay[key];

            foreach (var item in sessionTimePerDay)
            {
                var startTime = item.Item1 > tariffStartTime ? item.Item1 : tariffStartTime;
                var endTime = item.Item2 < tariffEndTime ? item.Item2 : tariffEndTime;

                var sessionDuration = endTime - startTime;
                var sessionDurationInMinutes = sessionDuration.TotalMinutes;

                price += (decimal)sessionDurationInMinutes * tariffPricePerMinute;
            }
        }

        return price;
    }

    private decimal GetTariffPricePerMinute(Tarif tariff)
    {
        switch (tariff.TimeUnitValue)
        {
            case TimeUnitValue.Minute:
                return tariff.PricePerTimeUnit;
            case TimeUnitValue.Hour:
                return tariff.PricePerTimeUnit / MINUTES_IN_HOUR;
            case TimeUnitValue.Day:
                return tariff.PricePerTimeUnit / HOURES_IN_DAY / MINUTES_IN_HOUR;
            default:
                throw new ParkyException("UNKNOWN_TIME_UNIT_VALUE");
        }
    }
}
