using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class ParkingSettingsConfiguration : IEntityTypeConfiguration<ParkingSettings>
{
    public void Configure(EntityTypeBuilder<ParkingSettings> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
