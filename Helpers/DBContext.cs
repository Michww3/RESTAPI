using Microsoft.EntityFrameworkCore;
using RESTAPI.DTOs;

public class DBContext : DbContext
{
    public DbSet<Person> Persons { get; set; } = null!;
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = "Tom", Age = 37 },
                new Person { Id = 2, Name = "Bob", Age = 41 },
                new Person { Id = 3, Name = "Sam", Age = 24 }
        );
    }
}
