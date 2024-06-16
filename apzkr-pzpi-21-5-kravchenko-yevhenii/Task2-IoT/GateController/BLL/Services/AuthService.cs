using BLL.Contracts;
using Domain.Models;

namespace BLL.Services;
public class AuthService : IAuthService
{
    private readonly IClient _client;

    public AuthService(IClient client)
    {
        _client = client;
    }

    public Token Login(LoginModel loginModel)
    {
        var token = _client.Login(loginModel);

        return token;
    }

    public void Logout()
    {
        _client.Logout();
    }
}
