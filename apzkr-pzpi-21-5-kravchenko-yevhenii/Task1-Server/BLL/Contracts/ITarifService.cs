using BLL.Infrastructure.Models.Tarif;

namespace BLL.Contracts;
public interface ITarifService
{
    IEnumerable<TarifModel> GetAll();

    TarifModel GetById(int tarifId);

    void Add(TarifModel tarifModel);

    void Update(TarifModel tarifModel);

    void Delete(int tarifId); 
}
