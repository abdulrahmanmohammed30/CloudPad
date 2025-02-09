using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Models;

namespace NoteTakingApp.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public DbSet<Country?> Countries { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override  void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}