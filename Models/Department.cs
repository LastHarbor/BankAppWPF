using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BankApp.Data;
using BankApp.ViewModels.Base;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Models;

public class Department
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ObservableCollection<Client>? Clients { get; set; } = new();

}

public class Singleton
{
    private static readonly Singleton Instance = new();
    private readonly ObservableCollection<Department> _departments;
    private readonly DataContext _db = new();

    private Singleton()
    {
            _departments = new ObservableCollection<Department>();
            _db.Clients.Load();
            _db.Departments.Load();
            _departments = _db.Departments.Local.ToObservableCollection();
    }

    public static Singleton GetInstance()
    {
        return Instance;
    }

    public ObservableCollection<Department> GetDepartments()
    {
        return _departments;
    }

    public void AddDepartment(Department department)
    {
        _departments.Add(department);
        _db.Departments.Add(department);
        _db.SaveChanges();
    }

}