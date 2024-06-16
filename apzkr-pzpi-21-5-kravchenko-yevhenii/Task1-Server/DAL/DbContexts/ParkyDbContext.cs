using Domain.Models;
using Infrastructure.Enums;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DAL.DbContexts;
public class ParkyDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<UserProfile> UserProfiles { get; set; } = null!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public DbSet<ParkingPlace> ParkingPlaces { get; set; } = null!;

    public DbSet<ParkingSession> ParkingSessions { get; set; } = null!;

    public DbSet<Booking> Bookings { get; set; } = null!;

    public DbSet<Payment> Payments { get; set; } = null!;

    public DbSet<ParkingSettings> ParkingSettings { get; set; } = null!;

    public ParkyDbContext(DbContextOptions<ParkyDbContext> options)
        :base(options)
    {
        //Database.EnsureCreated();
        //Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParkyDbContext).Assembly);
        AddSystemAdmin(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void AddSystemAdmin(ModelBuilder modelBuidler)
    {
        AddSystemAdminUser(modelBuidler);
        AddSystemAdminProfile(modelBuidler);

        AddDefaultParkingSettings(modelBuidler);
    }

    private void AddSystemAdminUser(ModelBuilder modelBuidler)
    {
        modelBuidler.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Login = "Admin",
                PasswordHash = "$2a$10$Usqo0Y9zHoU0Wpt5jZl2B.DKMaKZ7cT/tDDLTv9vwsj7PlaWs40Ai", // password: 12345
                PasswordSalt = "G+94*Wzt@_e!GOFQ",
                RegistrationDate = DateTime.UtcNow,
                Role = Role.SystemAdmin
            }
        );
    }

    private void AddSystemAdminProfile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>().HasData(
            new UserProfile
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                ProfilePicture = Array.Empty<byte>(),
                Address = "Street Ave. 15",
                PhoneNumber = "0555555555",
                BirthDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Email = "admin@bibliworm.com"
            }
        );
    }

    private void AddDefaultParkingSettings(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingSettings>().HasData(
            new ParkingSettings
            {
                Id = 1,
                BookingTimeAdvanceInMinutes = 20,
                BookingDurationInMinutes = 20
            });
    }
}
