using System.IO;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
namespace BankApp.Data;

public class DataContext : DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Client> Clients { get; set; }

    public DataContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("BankDB");
        optionsBuilder.UseSqlite(connectionString); //Сдлеать через файл строки подключения.
    }

}