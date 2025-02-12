using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Infrastructure.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.Property(b=>b.ResourceId).ValueGeneratedOnAdd();

            builder.HasKey(r => r.ResourceId);

            builder.Property(r => r.FilePath)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(r => r.DisplayName)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(r => r.Description)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(r => r.Size)
                .HasColumnType("BIGINT")
                .IsRequired(false);

            builder.Property(r => r.CreatedAt)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.Property(r => r.UpdatedAt)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

        }
    }
}


