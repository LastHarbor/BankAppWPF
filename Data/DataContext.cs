using BankApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;

namespace BankApp.Data;

public class DataContext : DbContext
{
    public DbSet<Department> Departments { get; set; } = null;
    public DbSet<Client> Clients { get; set; } = null;

    public DataContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite("Data Source=Data/BankApp.db");
    
}