using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Models;

public class Contexts : DbContext
{
    public Contexts(DbContextOptions<Contexts> options) : base(options) { }
   
    public DbSet<Book>? Books { get; set; }
    public DbSet<Member>? Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = @"Data Source="+Environment.CurrentDirectory + @"\wwwroot\Database.sqlite";
        optionsBuilder.UseSqlite(connectionString);
    }
}
