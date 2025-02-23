using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Infrastructure.Configurations;

public class SocialLinkConfiguration:IEntityTypeConfiguration<UserSocialLink>
{
    public void Configure(EntityTypeBuilder<UserSocialLink> builder)
    {
        builder.HasKey(b => b.UserSocialLinkId);
        
        builder.Property(b => b.UserSocialLinkId)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(b=>b.Url)
            .HasColumnType("nvarchar(max)")
            .IsRequired();
    }
}

