using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
public class ResetPasswordTokenConfiguration : IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> builder)
    {
        builder.HasKey(rpt => rpt.Id);
    }
}
