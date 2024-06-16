using BLL.Infrastructure.Models.Car;

namespace BLL.Contracts;
public interface ICarService
{
    void Add(CarModel carModel);

    IEnumerable<CarModel> GetUserCars(int userId);

    void Delete(int carId);

    void Update(CarModel carModel);

    CarModel GetById(int carId);
}
