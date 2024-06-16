using Domain.Models;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BLL.Infrastructure.Filters;
public class UserFilter : BaseFilter<User>
{
    public string? SearchQuery { get; set; }

    public override IQueryable<User> Filter(IQueryable<User> users)
    {
        IQueryable<User> query = users
            .Include(u => u.UserProfile);

        if (!SearchQuery.IsNullOrWhiteSpace())
        {
            query = query.Where(u => u.Login.StartsWith(SearchQuery!)
                || (u.UserProfile.LastName + " " + u.UserProfile.FirstName).StartsWith(SearchQuery!)
                || u.UserProfile.PhoneNumber!.StartsWith(SearchQuery!)
                || u.UserProfile.Email!.StartsWith(SearchQuery!)).AsQueryable();
        }

        return query;
    }
}
