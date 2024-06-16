using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class ParkingPlaceConfiguration : IEntityTypeConfiguration<ParkingPlace>
{
    public void Configure(EntityTypeBuilder<ParkingPlace> builder)
    {
        builder.HasKey(pp => pp.Id);
        builder.HasMany(pp => pp.ParkingSessions)
            .WithOne(ps => ps.ParkingPlace)
            .HasForeignKey(ps => ps.ParkingPlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
