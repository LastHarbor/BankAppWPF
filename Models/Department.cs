using System.Collections.ObjectModel;
using BankApp.ViewModels.Base;

namespace BankApp.Models;

public class Department
{
    public string Name { get; set; }
    public ObservableCollection<Client> Clients { get; set; }

    public class DepartmentExtensions : ViewModel
    {
        private ObservableCollection<Department> _departments = new ObservableCollection<Department>();

        public ObservableCollection<Department> Departments
        {
            get =>  _departments;
            set => SetField(ref _departments, value);
        }

        public void AddDepartment(Department department)
        {
            Departments.Add(department);
        }

        public void RemoveDepartment(Department department)
        {
            Departments.Remove(department);
        }
    }
}