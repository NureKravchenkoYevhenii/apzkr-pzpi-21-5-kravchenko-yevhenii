using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class ParkingSessionConfiguration : IEntityTypeConfiguration<ParkingSession>
{
    public void Configure(EntityTypeBuilder<ParkingSession> builder)
    {
        builder.HasKey(ps => ps.Id);
        builder.ToTable(table => table.HasCheckConstraint(
            "CHK_EndDate_ParkingSessions", "[EndDate] > [StartDate]"));
    }
}
