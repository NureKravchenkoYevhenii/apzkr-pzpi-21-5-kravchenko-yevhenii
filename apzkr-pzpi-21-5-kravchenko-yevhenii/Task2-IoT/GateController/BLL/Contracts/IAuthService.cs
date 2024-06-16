using Domain.Models;

namespace BLL.Contracts;
public interface IAuthService
{
    Token Login(LoginModel loginModel);

    void Logout();
}
