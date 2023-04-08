using System.Collections.ObjectModel;
using System.Linq;
using BankApp.ViewModels.Base;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Models;

public class Department
{
    public string? Name { get; set; }
    public ObservableCollection<Client>? Clients { get; set; }

    public Department()
    {

    }
}

public class Singleton
{
    private static readonly Singleton Instance = new();
    private readonly ObservableCollection<Department> _departments;

    private Singleton()
    {
        _departments = new ObservableCollection<Department>();
        if(_departments != null)
        {using (var context = new DataContext())
        {
            var departments = context.Departments.Include(d => d.Clients).ToList();
            foreach (var department in departments)
            {
                if (_departments.Any(d => d.Id == department.Id)) continue;
                _departments.Add(department);
            }
        }}
        
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
    }

}