using Domain.Models;

namespace BLL.Contracts;
public interface IClient
{
    Token Login(LoginModel loginModel);

    bool CheckIn();

    bool CheckOut();

    void Logout();
}
