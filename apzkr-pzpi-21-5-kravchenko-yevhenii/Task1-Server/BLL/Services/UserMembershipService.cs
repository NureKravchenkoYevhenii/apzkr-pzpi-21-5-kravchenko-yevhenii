using BLL.Contracts;
using BLL.Infrastructure.Models.Membership;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Exceptions;

namespace BLL.Services;
public class UserMembershipService : IUserMembershipService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    private readonly Lazy<IRepository<UserMembership>> _userMemberships;
    private readonly Lazy<IRepository<Membership>> _memberships;
    private readonly Lazy<IRepository<User>> _users;
    public UserMembershipService(
        Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;

        _userMemberships = _unitOfWork.Value
            .GetLazyRepository<UserMembership>();
        _memberships = _unitOfWork.Value
            .GetLazyRepository<Membership>();
        _users = _unitOfWork.Value
            .GetLazyRepository<User>();
    }

    public void Add(int membershipId, int userId)
    {
        var membership = _memberships.Value.GetById(membershipId)
            ?? throw new EntityNotFoundException("MEMBERSHIP_NOT_FOUND");
        var user = _users.Value.GetById(userId)
            ?? throw new EntityNotFoundException("USER_NOT_FOUND");

        _userMemberships.Value.Add(new UserMembership
        {
            MembershipId = membership.Id,
            UserId = user.Id,
            StartDate = DateTime.UtcNow
        });
    }

    public void BlockUserMembership(BlockMembershipModel blockMembershipModel)
    {
        var userMembership = _userMemberships.Value.GetById(blockMembershipModel.UserMembershipId)
            ?? throw new EntityNotFoundException("USER_MEMBERSHIP_NOT_FOUND");

        userMembership.IsBlocked = blockMembershipModel.IsBlocked;
        if (blockMembershipModel.IsBlocked)
        {
            userMembership.Comment = blockMembershipModel.Comment;
        }
        else
        {
            userMembership.Comment = string.Empty;
        }

        _userMemberships.Value.Update(userMembership);
    }
}
