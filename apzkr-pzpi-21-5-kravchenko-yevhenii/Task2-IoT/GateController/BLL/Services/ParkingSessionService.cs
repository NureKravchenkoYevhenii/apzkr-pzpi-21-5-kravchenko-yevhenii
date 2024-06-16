using BLL.Contracts;

namespace BLL.Services;
public class ParkingSessionService : IParkingSessionService
{
    private readonly IClient _client;

    public ParkingSessionService(
        IClient client)
    {
        _client = client;
    }
    
    public bool CheckIn()
    {
        return _client.CheckIn();
    }

    public bool CheckOut()
    {
        return _client.CheckOut();
    }
}
