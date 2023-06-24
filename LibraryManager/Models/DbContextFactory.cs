using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryManager.Models;

public class DbContextFactory : IDesignTimeDbContextFactory<Contexts>
{
    public Contexts CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<Contexts>();
        string dir = @"Data Source=" + Environment.CurrentDirectory + @"\wwwroot\Database.sqlite";
        optionBuilder.UseSqlite(dir);
        return new Contexts(optionBuilder.Options);
    }
}
