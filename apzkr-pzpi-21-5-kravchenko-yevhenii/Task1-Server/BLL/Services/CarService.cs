using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Car;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Exceptions;

namespace BLL.Services;
public class CarService : ICarService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    private readonly Lazy<IRepository<Car>> _cars;

    public CarService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _cars = _unitOfWork.Value.GetLazyRepository<Car>();
    }

    public void Add(CarModel carModel)
    {
        var car = _mapper.Value.Map<Car>(carModel);

        _cars.Value.Add(car);
    }

    public void Delete(int carId)
    {
        var carToDelete = _cars.Value.GetById(carId);

        if (carToDelete == null)
            return;

        _cars.Value.Remove(carToDelete);
    }

    public CarModel GetById(int carId)
    {
        var car = _cars.Value.GetById(carId)
            ?? throw new EntityNotFoundException("CAR_NOT_FOUND");

        var carModel = _mapper.Value.Map<CarModel>(car);

        return carModel;
    }

    public IEnumerable<CarModel> GetUserCars(int userId)
    {
        var userCars = _cars.Value.GetAll()
            .Where(c => c.UserId == userId);

        var userCarModels = _mapper.Value.Map<List<CarModel>>(userCars);

        return userCarModels;
    }

    public void Update(CarModel carModel)
    {
        var car = _cars.Value.GetById(carModel.Id)
            ?? throw new EntityNotFoundException("CAR_NOT_FOUND");

        _mapper.Value.Map(carModel, car);

        _cars.Value.Update(car);
    }
}
