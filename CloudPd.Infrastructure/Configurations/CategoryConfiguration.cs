using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Entities.Domains;

namespace NoteTakingApp.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.CategoryId).ValueGeneratedOnAdd();

            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.CreatedAt)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.Property(c => c.IsFavorite)
                .HasDefaultValue(false);

            builder.HasOne<ApplicationUser>()
                .WithMany(u=>u.Categories)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.CategoryGuid)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .IsRequired();

            builder.HasIndex(c => c.Name).IsUnique();
            builder.HasIndex(c => c.CategoryGuid).IsUnique();

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}


