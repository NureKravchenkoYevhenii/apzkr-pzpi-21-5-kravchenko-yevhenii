using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.ParkingPlace;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;
public class ParkingPlaceService : IParkingPlaceService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;
    private readonly Lazy<IRepository<ParkingPlace>> _parkingPlaces;

    public ParkingPlaceService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _parkingPlaces = _unitOfWork.Value.GetLazyRepository<ParkingPlace>();
    }

    public void Add(ParkingPlaceModel parkingPlaceModel)
    {
        var parkingPlace = _mapper.Value.Map<ParkingPlace>(parkingPlaceModel);

        _parkingPlaces.Value.Add(parkingPlace);
    }

    public void Delete(int parkingPlaceId)
    {
        var parkingPlace = _parkingPlaces.Value.GetById(parkingPlaceId);
        if (parkingPlace == null)
            return;

        _parkingPlaces.Value.Remove(parkingPlace);
    }

    public IEnumerable<ParkingPlaceListModel> GetAll()
    {
        var parkingPlaces = _parkingPlaces.Value.GetAll().ToList();
        var parkingPlacesModel = _mapper.Value.Map<List<ParkingPlaceListModel>>(parkingPlaces);

        return parkingPlacesModel;
    }

    public ParkingPlaceModel GetById(int parkingPlaceId)
    {
        var parkingPlace = _parkingPlaces.Value.GetById(parkingPlaceId)
            ?? throw new EntityNotFoundException("PARKING_PLACE_NOT_FOUND");

        var parkingPlaceModel = _mapper.Value.Map<ParkingPlaceModel>(parkingPlace);

        return parkingPlaceModel;
    }

    public ParkingPlacesOccupancyModel GetOccupancyStatistics()
    {
        var result = new ParkingPlacesOccupancyModel();
        var parkingPlaces = _parkingPlaces.Value.GetAll()
            .Include(pp => pp.Bookings)
            .Include(pp => pp.ParkingSessions)
            .ToList();
        var occupiedParkingPlaces = parkingPlaces
            .Where(pp => pp.Bookings.Count == 0
                && !pp.ParkingSessions.Where(ps => ps.EndDate == null).Any())
            .ToList();

        result.Total = parkingPlaces.Count;
        result.Occupied = occupiedParkingPlaces.Count;

        return result;
    }

    public void Update(ParkingPlaceModel parkingPlaceModel)
    {
        var parkingPlace = _parkingPlaces.Value.GetById(parkingPlaceModel.Id)
            ?? throw new EntityNotFoundException("PARKING_PLACE_NOT_FOUND");

        _mapper.Value.Map(parkingPlaceModel, parkingPlace);

        _parkingPlaces.Value.Update(parkingPlace);
    }
}
