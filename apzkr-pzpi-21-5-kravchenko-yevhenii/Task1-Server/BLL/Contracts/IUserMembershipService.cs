using BLL.Infrastructure.Models.Membership;

namespace BLL.Contracts;
public interface IUserMembershipService
{
    void Add(int membershipId, int userId);

    void BlockUserMembership(BlockMembershipModel blockMembershipModel);
}
