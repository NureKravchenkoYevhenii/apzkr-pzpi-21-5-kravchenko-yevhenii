using BLL.Infrastructure.Models.ParkingSession;

namespace BLL.Contracts;
public interface IParkingSessionService
{
    bool CheckIn(string carNumber);

    bool CheckOut(string carNumber);

    IEnumerable<ParkingSessionModel> GetUserParkingSessions(int userId);

    byte[] GetParkingStatistics(DateTime from, DateTime to);
}
