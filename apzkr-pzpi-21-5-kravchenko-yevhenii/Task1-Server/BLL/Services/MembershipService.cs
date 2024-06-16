using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models;
using BLL.Infrastructure.Models.Membership;
using BLL.Infrastructure.Models.Payment;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Enums;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BLL.Services;
public class MembershipService : IMembershipService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;
    private readonly Lazy<IPaymentService> _paymentService;
    private readonly Lazy<IRepository<Membership>> _memberships;
    private readonly Lazy<IRepository<User>> _users;

    public MembershipService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper,
        Lazy<IPaymentService> paymentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paymentService = paymentService;
        _memberships = _unitOfWork.Value.GetLazyRepository<Membership>();
        _users = _unitOfWork.Value.GetLazyRepository<User>();
    }
    public void Add(MembershipModel membershipModel)
    {
        var membership = _mapper.Value.Map<Membership>(membershipModel);

        _memberships.Value.Add(membership);
    }

    public void Delete(int membershipId)
    {
        var membership = _memberships.Value.GetById(membershipId);
        if (membership == null)
            return;

        _memberships.Value.Remove(membership);
    }

    public IEnumerable<MembershipModel> GetAll()
    {
        var memberships = _memberships.Value.GetAll()
            .ToList();

        var membershipsModel = _mapper.Value.Map<List<MembershipModel>>(memberships);

        return membershipsModel;
    }

    public MembershipModel GetById(int membershipId)
    {
        var membership = _memberships.Value.GetById(membershipId)
            ?? throw new EntityNotFoundException("MEMBERSHIP_NOT_FOUND");

        var membershipModel = _mapper.Value.Map<MembershipModel>(membership);

        return membershipModel;
    }

    public async Task<string> PurchaseMembership(PurchaseMembershipModel purchaseMembershipModel)
    {
        var membership = GetById(purchaseMembershipModel.MembershipId);
        var user = _users.Value.GetAll()
            .Include(u => u.UserMemberships)
            .FirstOrDefault(u => u.Id == purchaseMembershipModel.UserId)
                ?? throw new EntityNotFoundException("USER_NOT_FOUND");

        if (user.UserMemberships.Any(um => um.StartDate.Add(TimeSpan.Zero) < DateTime.UtcNow
            && !um.IsBlocked))
        {
            throw new ParkyException("USER_ALREADY_HAS_MEMBESHIP");
        }

        var payloadModel = new PayloadModel
        {
            PurchaseType = PurchaseType.PurchaseMembership,
            PayItemId = membership.Id,
            UserId = user.Id,
        };
        var payload = JsonConvert.SerializeObject(payloadModel);

        var paymentString = await _paymentService.Value.MakePayment(
            membership.Price,
            payload);

        return paymentString;
    }

    public void Update(MembershipModel membershipModel)
    {
        var membership = _memberships.Value.GetById(membershipModel.Id)
            ?? throw new EntityNotFoundException("MEMBERSHIP_NOT_FOUND");

        _mapper.Value.Map(membershipModel, membership);

        _memberships.Value.Update(membership);
    }
}
