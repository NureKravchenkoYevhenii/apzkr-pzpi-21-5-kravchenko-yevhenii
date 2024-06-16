using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class UserConfiguration 
    : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasForeignKey<UserProfile>(up => up.Id)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u => u.ResetPasswordTokens)
            .WithOne(rpt => rpt.User)
            .HasForeignKey(rpt => rpt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u => u.Payments)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(u => u.ParkingSessions)
            .WithOne(ps => ps.User)
            .HasForeignKey(ps => ps.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u => u.UserMemberships)
            .WithOne(um => um.User)
            .HasForeignKey(um => um.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u => u.Cars)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
