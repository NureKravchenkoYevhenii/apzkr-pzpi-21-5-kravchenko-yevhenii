using BLL.Infrastructure.Models.ParkingPlace;

namespace BLL.Contracts;
public interface IParkingPlaceService
{
    IEnumerable<ParkingPlaceListModel> GetAll();

    ParkingPlaceModel GetById(int parkingPlaceId);

    void Add(ParkingPlaceModel parkingPlaceModel);

    void Update(ParkingPlaceModel parkingPlaceModel);

    void Delete(int parkingPlaceId);

    ParkingPlacesOccupancyModel GetOccupancyStatistics();
}
