using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.ToTable(table => table.HasCheckConstraint(
            "CHK_Sum_Payments",
            "[Sum] > 0"));

        builder.Property(p => p.Sum)
            .HasColumnType("decimal(18, 2)");
    }
}
