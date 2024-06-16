using Microsoft.EntityFrameworkCore;

namespace BLL.Infrastructure.Filters;
public class BaseFilter<TEntity>
    where TEntity : class
{
    public PagingModel PagingModel { get; set; } = new PagingModel();

    public virtual IQueryable<TEntity> Filter(IQueryable<TEntity> entities)
    {
        return entities;
    }
}
