namespace BLL.Contracts;
public interface IParkingSessionService
{
    bool CheckIn();

    bool CheckOut();
}
