using CloudPad.Core.Entities.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudPad.Core.Domains;

namespace CloudPad.Infrastructure.Configurations;

public class ApplicationUserConfiguration:IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(a => a.CreatedAt)
            .HasColumnType("DateTime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.Bio)
            .HasColumnType("nvarchar")
            .HasMaxLength(3000)
            .IsRequired(false);

        builder.HasOne(b => b.PreferredLanguage)
            .WithMany()
            .HasForeignKey(b => b.PreferredLanguageId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b=>b.SocialLinks)
            .WithOne(s=>s.User)
            .HasForeignKey(s=>s.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}