using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Extensions;
using BLL.Infrastructure.Filters;
using BLL.Infrastructure.Models;
using BLL.Infrastructure.Models.UserMembership;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Configs;
using Infrastructure.Enums;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Resources;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security.Cryptography;

namespace BLL.Services;
public class UserService : IUserService
{
    private readonly Lazy<AuthOptions> _authOptions;
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;
    private readonly Lazy<IRepository<User>> _users;
    private readonly Lazy<IRepository<ResetPasswordToken>> _resetPasswordTokens;
    private readonly Lazy<IRepository<UserProfile>> _userProfiles;
    private readonly Lazy<IRepository<UserMembership>> _userMemberships;

    public UserService(
        Lazy<AuthOptions> authOptions,
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper)
    {
        _authOptions = authOptions;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _users = _unitOfWork.Value.GetLazyRepository<User>();
        _resetPasswordTokens = _unitOfWork.Value.GetLazyRepository<ResetPasswordToken>();
        _userProfiles = _unitOfWork.Value.GetLazyRepository<UserProfile>();
        _userMemberships = _unitOfWork.Value.GetLazyRepository<UserMembership>();
    }

    public Guid CreateRefreshToken(int userId)
    {
        var user = _users.Value.GetAll()
            .Include(u => u.RefreshTokens)
            .FirstOrDefault(u => u.Id == userId) 
                ?? throw new EntityNotFoundException("USER_NOT_FOUND");

        var refreshToken = new RefreshToken()
        {
            Token = Guid.NewGuid(),
            UserId = userId,
            ExpiresOnUtc = DateTime.UtcNow
                .AddSeconds(_authOptions.Value.RefreshTokenLifetime),
        };

        user.RefreshTokens.Clear();
        user.RefreshTokens.Add(refreshToken);

        _unitOfWork.Value.Commit();

        return refreshToken.Token;
    }

    public IEnumerable<UserProfileModel> GetAll(UserFilter filter)
    {
        var users = _users.Value.GetAll()
            .Include(u => u.UserProfile);
        var filteredUsers = filter.Filter(users)
            .OrderBy(u => u.Id)
            .GetPage(filter.PagingModel)
            .ToList();

        var userProfiles = _mapper.Value.Map<List<UserProfileModel>>(filteredUsers);

        return userProfiles;
    }

    public UserModel GetUserByRefreshToken(Guid refreshToken)
    {
        var user = _users.Value.GetAll()
            .Include(u => u.RefreshTokens)
            .FirstOrDefault(u => u.RefreshTokens.Any(rt =>
                rt.Token == refreshToken
                    && rt.ExpiresOnUtc >= DateTime.UtcNow)
            ) ?? throw new ParkyException("INVALID_REFRESH_TOKEN");

        var userModel = _mapper.Value.Map<UserModel>(user);

        return userModel;
    }

    public UserProfileModel GetUserProfileById(int userId)
    {
        var user = _users.Value.GetAll()
            .Include(u => u.UserProfile)
            .FirstOrDefault(u => u.Id == userId)
                ?? throw new EntityNotFoundException("USER_NOT_FOUND");

        var userProfile = _mapper.Value.Map<UserProfileModel>(user);
        FillUserMembershipModel(userProfile);

        return userProfile;
    }

    public UserModel GetByEmail(string emailAddess)
    {
        var userProfile = _users.Value.Get(p => p.UserProfile.Email == emailAddess) 
            ?? throw new EntityNotFoundException("USER_WITH_EMAIL_NOT_FOUND");

        var userModel = _mapper.Value.Map<UserModel>(userProfile);

        return userModel;
    }

    public bool IsResetPasswordTokenValid(string token)
    {
        var resetPasswordToken = _resetPasswordTokens.Value.Get(rpt => rpt.Token == token
            && rpt.ExpiresOnUtc >= DateTime.UtcNow);

        return resetPasswordToken != null;
    }

    public UserModel LoginUser(string login, string password)
    {
        var user = _users.Value.Get(u => u.Login == login
            || u.UserProfile.Email == login);

        if (user is null
            || !HashHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("INVALID_LOGIN_OR_PASSWORD");
        }

        var userModel = _mapper.Value.Map<UserModel>(user);

        return userModel;
    }

    public void RegisterUser(RegisterUserModel model)
    {
        if (_users.Value.GetAll().Any(x => x.Login == model.Login))
            throw new ParkyException("LOGIN_EXISTS");

        if (_users.Value.GetAll().Any(x => x.UserProfile.Email == model.Email))
            throw new ParkyException("EMAIL_EXISTS");

        var (salt, passwordHash) = HashHelper.GenerateNewPasswordHash(model.Password);
        var user = _mapper.Value.Map<User>(model);
        var userProfile = _mapper.Value.Map<UserProfile>(model);
        if (userProfile.ProfilePicture == null)
            userProfile.ProfilePicture = Array.Empty<byte>();

        user.PasswordSalt = salt;
        user.PasswordHash = passwordHash;
        user.RegistrationDate = DateTime.UtcNow;
        user.UserProfile = userProfile;
        user.Role = Role.User;

        _users.Value.Add(user);
    }

    public void ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        var user = _users.Value.GetAll()
            .Include(u => u.ResetPasswordTokens)
            .FirstOrDefault(u => u.ResetPasswordTokens
                .Any(t => t.Token == resetPasswordModel.Token
                        && t.ExpiresOnUtc >= DateTime.UtcNow))
            ?? throw new ParkyException("INVALID_RESET_PASSWORD_TOKEN");

        var (salt, passwordHash) = HashHelper.GenerateNewPasswordHash(resetPasswordModel.Password);
        user.PasswordSalt = salt;
        user.PasswordHash = passwordHash;
        user.ResetPasswordTokens.Clear();

        _unitOfWork.Value.Commit();
    }

    public void SetUserRole(int userId, Role role)
    {
        var user = _users.Value.GetById(userId)
            ?? throw new EntityNotFoundException("USER_NOT_FOUND");

        user.Role = role;

        _unitOfWork.Value.Commit();
    }

    public void UpdateUserProfile(UserProfileInfo model)
    {
        var profile = _userProfiles.Value.Get(p => p.Id == model.Id)
            ?? throw new EntityNotFoundException("USER_PROFILE_NOT_FOUND");

        _mapper.Value.Map(model, profile);
        _unitOfWork.Value.Commit();
    }

    public string GenerateResetPasswordToken(int userId)
    {
        var user = _users.Value.GetAll()
            .Include(u => u.ResetPasswordTokens)
            .FirstOrDefault(u => u.Id == userId)
                ?? throw new EntityNotFoundException("USER_NOT_FOUND");

        var token = CreateRandomToken();
        var resetPasswordToken = new ResetPasswordToken()
        {
            Token = token,
            UserId = userId,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(1),
        };

        user.ResetPasswordTokens.Clear();
        user.ResetPasswordTokens.Add(resetPasswordToken);

        _unitOfWork.Value.Commit();

        return token;
    }

    public void UploadUserData(Stream fileData)
    {
        try
        {
            using (var package = new ExcelPackage(fileData))
            {
                var workSheet = package.Workbook.Worksheets[0];
                var rowCount = workSheet.Dimension.Rows;

                for (int i = 2; i <= rowCount; i++)
                {
                    var user = new User();

                    var password = workSheet.Cells[i, 3].Value.ToString()!;
                    var (salt, passwordHash) = HashHelper.GenerateNewPasswordHash(password);

                    user.Login = workSheet.Cells[i, 2].Value.ToString();
                    user.PasswordSalt = salt;
                    user.PasswordHash = passwordHash;
                    user.RegistrationDate = DateTime.UtcNow;
                    user.Role = Role.User;

                    var userProfile = new UserProfile();

                    userProfile.FirstName = workSheet.Cells[i, 4].Value.ToString();
                    userProfile.LastName = workSheet.Cells[i, 5].Value.ToString();
                    userProfile.Address = workSheet.Cells[i, 6].Value.ToString();
                    userProfile.PhoneNumber = workSheet.Cells[i, 7].Value.ToString();
                    userProfile.BirthDate = DateTime.Parse(workSheet.Cells[i, 8].Value.ToString()!);
                    userProfile.Email = workSheet.Cells[i, 9].Value.ToString();

                    var carNumbers = workSheet.Cells[i, 10].Value.ToString()?.Split(",") ?? Array.Empty<string>();
                    user.Cars = new List<Car>();

                    foreach (var carNumber in carNumbers)
                        user.Cars.Add(new Car { CarNumber = carNumber.Trim() });

                    user.UserProfile = userProfile;

                    _users.Value.Add(user);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"{Resources.Get("USER_DATA_PARSE_FAILURE")}{ex.Message}");
        }
    }

    private string CreateRandomToken()
    {
        var newToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        while (_resetPasswordTokens.Value.GetAll().Any(t => t.Token == newToken))
            newToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        return newToken;
    }

    private void FillUserMembershipModel(UserProfileModel userProfile)
    {
        var notExpiredUserMembership = _userMemberships.Value.GetAll()
            .Include(um => um.Membership)
            .FirstOrDefault(um => um.UserId == userProfile.Id 
                && um.StartDate.AddDays(um.Membership.DurationInDays) > DateTime.UtcNow);

        if (notExpiredUserMembership == null)
            return;

        var userMembershipModel = _mapper.Value.Map<UserMembershipModel>(notExpiredUserMembership);
        userProfile.UserMembershipModel = userMembershipModel;
    }
}
