using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Tarif;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Exceptions;

namespace BLL.Services;
public class TarifService : ITarifService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    private readonly Lazy<IRepository<Tarif>> _tarifs;

    public TarifService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _tarifs = _unitOfWork.Value.GetLazyRepository<Tarif>();
    }

    public void Add(TarifModel tarifModel)
    {
        var tarifs = GetAll();

        foreach (var tarif in tarifs)
        {
            if (AreTarifsIntersectOnDayOfWeek(tarifModel, tarif, out var intersectDay))
            {
                if (tarifModel.EndTime >= tarif.StartTime && tarifModel.EndTime <= tarif.EndTime
                    || tarifModel.StartTime >= tarif.StartTime && tarifModel.StartTime <= tarif.EndTime)
                {
                    throw new ParkyException("TARIFS_INTERSECT");
                }
            }
        }

        var tarifToAdd = _mapper.Value.Map<Tarif>(tarifModel);

        _tarifs.Value.Add(tarifToAdd);
    }

    public void Delete(int tarifId)
    {
        var tarif = _tarifs.Value.GetById(tarifId);
        if (tarif == null)
            return;

        _tarifs.Value.Remove(tarif);
    }

    public IEnumerable<TarifModel> GetAll()
    {
        var tarifs = _tarifs.Value.GetAll();
        var tarifModels = _mapper.Value.Map<List<TarifModel>>(tarifs);

        return tarifModels;
    }

    public TarifModel GetById(int tarifId)
    {
        var tarif = _tarifs.Value.GetById(tarifId)
            ?? throw new EntityNotFoundException("TARIF_NOT_FOUND");

        var tarifModel = _mapper.Value.Map<TarifModel>(tarif);

        return tarifModel;
    }

    public void Update(TarifModel tarifModel)
    {
        var tarifToUpdate = _tarifs.Value.GetById(tarifModel.Id)
            ?? throw new EntityNotFoundException("TARIF_NOT_FOUND");

        var tarifs = GetAll().Where(t => t.Id != tarifModel.Id);

        foreach (var tarif in tarifs)
        {
            if (AreTarifsIntersectOnDayOfWeek(tarifModel, tarif, out var intersectDay))
            {
                if (tarifModel.EndTime >= tarif.StartTime && tarifModel.EndTime <= tarif.EndTime
                    || tarifModel.StartTime >= tarif.StartTime && tarifModel.StartTime <= tarif.EndTime)
                {
                    throw new ParkyException("TARIFS_INTERSECT");
                }
            }
        }

        _mapper.Value.Map(tarifModel, tarifToUpdate);
        _tarifs.Value.Update(tarifToUpdate);
    }

    private bool AreTarifsIntersectOnDayOfWeek(
        TarifModel firstTarif,
        TarifModel secondTarif,
        out DayOfWeek intersectDay)
    {
        foreach (var firstTarifActiveDay in firstTarif.ActiveOnDaysOfWeek)
        {
            foreach (var secondTarifActiveDay in secondTarif.ActiveOnDaysOfWeek)
            {
                if (firstTarifActiveDay == secondTarifActiveDay)
                {
                    intersectDay = firstTarifActiveDay;
                    return true;
                }
            }
        }

        intersectDay = DayOfWeek.Monday;
        
        return false;
    }
}
