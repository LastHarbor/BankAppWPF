using System.Collections.ObjectModel;
using System.Linq;
using BankApp.Models;
using System.Windows.Input;
using BankApp.Data;
using BankApp.Extensions;

namespace BankApp.ViewModels.Base;

public class AddDepartmentViewModel : ViewModel
{
    private DataContext _context;
    private int _id;
    public int Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    private string? _departmentName;
    public string? DepartmentName
    {
        get => _departmentName;
        set => SetField(ref _departmentName, value);
    }

    
    public ICommand AddDepartmentCommand { get; }
    private void OnAddDepartmentCommand(object p)
    {
        var department = new Department()
        {
            Name = DepartmentName,
            Id = Id,
        };
        using (_context = new DataContext())
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }
        Extensions.Extensions.CloseDialog();
    }
    private bool CanAddDepartmentCommand(object p) => true;


    public AddDepartmentViewModel()
    {
        AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
    }

   
}