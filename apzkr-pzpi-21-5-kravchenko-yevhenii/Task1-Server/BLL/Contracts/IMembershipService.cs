using BLL.Infrastructure.Models;
using BLL.Infrastructure.Models.Membership;

namespace BLL.Contracts;
public interface IMembershipService
{
    IEnumerable<MembershipModel> GetAll();

    MembershipModel GetById(int membershipId);

    void Add(MembershipModel membershipModel);

    void Update(MembershipModel membershipModel);

    void Delete(int membershipId);

    Task<string> PurchaseMembership(PurchaseMembershipModel purchaseMembershipModel);
}
