using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Entities.Domains;

namespace NoteTakingApp.Infrastructure.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.TagId);

            builder.Property(t => t.Name)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasColumnType("NVARCHAR")
                .HasMaxLength (500)
                .IsRequired(false);

            builder.Property(t => t.CreatedAt)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
               .HasColumnType("DATETIME2")
               .HasDefaultValueSql("SYSUTCDATETIME()")
               .IsRequired();

            builder.HasMany(t => t.Notes)
                .WithMany(n => n.Tags)
                .UsingEntity("NoteTags");

            builder.HasOne<ApplicationUser>()
            .WithMany(u => u.Tags)
            .HasForeignKey(t => t.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(t => t.Name).IsUnique();

            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}
