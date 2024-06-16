
using BLL.Contracts;

namespace Parky.Infrastructure.HostedServices;

public class ExpiredBookingsCleaner : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IBookingService _bookingService;

    public ExpiredBookingsCleaner(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DeleteExpitedBookings, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        
    }

    private void DeleteExpitedBookings(object? state)
    {
        _bookingService.DeleteExpiredBookings();
    }
}
