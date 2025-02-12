using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Models;
using NoteTakingApp.Infrastructure.Configurations;

namespace NoteTakingApp.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<ApplicationUserWithRole> EnrichedUsers { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Resource> Resources { get; set; }

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