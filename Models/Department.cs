using System.Collections.ObjectModel;
using BankApp.ViewModels.Base;

namespace BankApp.Models;

public class Department
{
    public string? Name { get; set; }
    public ObservableCollection<Client>? Clients { get; set; }
}

public class Singleton
{
    private static readonly Singleton _instance = new();
    private readonly ObservableCollection<Department> _departments;

    private Singleton()
    {
        _departments = new ObservableCollection<Department>();
    }

    public static Singleton GetInstance()
    {
        return _instance;
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