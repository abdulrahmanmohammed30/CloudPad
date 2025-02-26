using CloudPad.Core.Configurations;
using CloudPad.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace CloudPad.Infrastructure.Configurations;

public class LanguageConfiguration(IOptions<List<LanguageData>> languageDataOptions) : IEntityTypeConfiguration<Language>
{
    private List<LanguageData> _languagesData => languageDataOptions.Value;
    
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(b => b.LanguageId);
        
        builder.Property(b => b.LanguageId)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(b=>b.Code)
            .HasColumnType("varchar")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(b => b.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(b=>b.NativeName)
            .HasColumnType("nvarchar")
            .HasMaxLength(80)
            .IsRequired();

        builder.HasIndex(b => b.Code).IsUnique();
        builder.HasIndex(b => b.Name).IsUnique();
        builder.HasIndex(b=>b.NativeName).IsUnique();

        builder.HasData(GetLanguages(_languagesData));
    }

    private List<Language> GetLanguages(List<LanguageData> languagesData)
    {
        int count = 1;
        List<Language> languages = new List<Language>();   
        foreach (var language in languagesData)
        {
            languages.Add(new Language()
            {
                LanguageId = count++,
                Code = language.Code,
                Name = language.Name,
                NativeName = language.NativeName
            });
        }
        return languages;
    }
}

