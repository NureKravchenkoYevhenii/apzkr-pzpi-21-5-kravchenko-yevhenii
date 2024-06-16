using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class TarifConfiguration : IEntityTypeConfiguration<Tarif>
{
    public void Configure(EntityTypeBuilder<Tarif> builder)
    {
        builder.HasKey(t => t.Id);
        builder.ToTable(table => table.HasCheckConstraint(
            "CHK_EndTime_Tarifs",
            "[EndTime] > [StartTime]"));
        builder.ToTable(table => table.HasCheckConstraint(
            "CHK_PricePerTimeUnit_Tarifs",
            "[PricePerTimeUnit] >= 0"));

        builder.Property(t => t.PricePerTimeUnit)
            .HasColumnType("decimal(18, 2)");
    }
}
