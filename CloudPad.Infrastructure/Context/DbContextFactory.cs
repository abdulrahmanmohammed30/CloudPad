using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CloudPad.Infrastructure.Context;

public class DbContextFactory:IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    { 
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
       DbContextOptionsBuilder<AppDbContext> optionsBuilder=new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(config.GetConnectionString("DefaultConnection"));
       return new AppDbContext(optionsBuilder.Options);
    }
}