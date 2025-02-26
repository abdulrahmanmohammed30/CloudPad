using CloudPad.Core.Entities;
using CloudPad.Core.Entities.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudPad.Core.Domains;

namespace CloudPad.Infrastructure.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.Property(n => n.NoteId).ValueGeneratedOnAdd();

            builder.HasKey(n => n.NoteId);

            builder.Property(n => n.Title)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(n => n.Content)
                .HasColumnType("NVARCHAR(MAX)")
                .IsRequired(false);

            builder.Property(n => n.CreatedAt)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.Property(n => n.UpdatedAt)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(n => n.Tags)
                .WithMany(t => t.Notes)
                .UsingEntity<Dictionary<string, object>>(
                    "NoteTags",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagsTagId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Note>().WithMany().HasForeignKey("NotesNoteId").OnDelete(DeleteBehavior.Cascade)
                );

            builder.HasOne(n => n.Category)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(n => n.Resources)
                .WithOne(r => r.Note)
                .HasForeignKey(r => r.NoteId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(n => n.NoteGuid)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .IsRequired();

            builder.HasIndex(n => n.NoteGuid).IsUnique();

            builder.Navigation(n => n.Tags).AutoInclude();
            builder.Navigation(n => n.Category).AutoInclude();

            builder.HasQueryFilter(n => !n.IsDeleted);
        }
    }
}