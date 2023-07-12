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
    private string _title;
    private string? _departmentName;


    public string Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    public int Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string? DepartmentName
    {
        get => _departmentName;
        set => SetField(ref _departmentName, value);
    }

    public ICommand CancelCommand { get; }
    public ICommand AddDepartmentCommand { get; }
    private bool CanCancelCommand(object p) => true;
    private bool CanAddDepartmentCommand(object p) => true;

    public AddDepartmentViewModel()
    {
        CancelCommand = new LambdaCommand(OnCancelCommand, CanCancelCommand);
        AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
    }
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
    private void OnCancelCommand(object p)
    {
        Extensions.Extensions.CloseDialog();
    }
}