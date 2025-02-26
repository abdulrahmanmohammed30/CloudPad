using CloudPad.Core.Domains;
using CloudPad.Core.Entities;
using CloudPad.Core.Entities.Domains;
using CloudPad.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CloudPad.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<ApplicationUserWithRole> EnrichedUsers { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Resource> Resources { get; set; }
    
    public DbSet<Language> Languages { get; set; }

    public DbSet<UserSocialLink> UserSocialLinks { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

protected override  void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUserWithRole>().ToView("UserWithRole");
        builder.Entity<ApplicationUserWithRole>().HasNoKey();
        builder.ApplyConfigurationsFromAssembly(typeof(NoteConfiguration).Assembly);

    }
}