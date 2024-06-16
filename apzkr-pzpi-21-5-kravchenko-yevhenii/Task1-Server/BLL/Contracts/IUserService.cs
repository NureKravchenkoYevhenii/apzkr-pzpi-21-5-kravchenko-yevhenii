using BLL.Infrastructure.Filters;
using BLL.Infrastructure.Models;
using Infrastructure.Enums;

namespace BLL.Contracts;
public interface IUserService
{
    UserProfileModel GetUserProfileById(int userId);

    UserModel GetByEmail(string emailAddess);

    IEnumerable<UserProfileModel> GetAll(UserFilter filter);

    void UpdateUserProfile(UserProfileInfo model);

    UserModel LoginUser(string login, string password);

    void RegisterUser(RegisterUserModel model);

    void ResetPassword(ResetPasswordModel resetPasswordModel);

    Guid CreateRefreshToken(int userId);

    UserModel GetUserByRefreshToken(Guid refreshToken);

    bool IsResetPasswordTokenValid(string token);

    string GenerateResetPasswordToken(int userId);

    void SetUserRole(int userId, Role role);

    void UploadUserData(Stream fileData);
}
