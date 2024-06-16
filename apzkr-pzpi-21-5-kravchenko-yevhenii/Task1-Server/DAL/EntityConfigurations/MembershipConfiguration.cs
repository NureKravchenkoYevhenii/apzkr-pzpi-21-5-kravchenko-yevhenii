using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.HasKey(m => m.Id);
        builder.ToTable(table => table.HasCheckConstraint(
            "CHK_Price_Memberships",
            "[Price] >= 0"));

        builder.HasMany(m => m.UserMemberships)
            .WithOne(um => um.Membership)
            .HasForeignKey(um => um.MembershipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.Price)
            .HasColumnType("decimal(18, 2)");
    }
}
