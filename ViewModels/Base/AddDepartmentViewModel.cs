using System.Collections.ObjectModel;
using System.Linq;
using BankApp.Models;
using System.Windows.Input;
using BankApp.Data;
using BankApp.Extensions;

namespace BankApp.ViewModels.Base;

public class AddDepartmentViewModel : ViewModel
{
    #region DepartmentExtension

    #endregion
    private int _id;

    public int ID
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

    #region AddDepartmentCommand

    public ICommand AddDepartmentCommand { get; }
    private void OnAddDepartmentCommand(object p)
    {
        var department = new Department()
        {
            Name = DepartmentName,
            Id = ID,
        };
        Singleton.GetInstance().AddDepartment(department);
        Extensions.Extensions.CloseDialog();
    }
    private bool CanAddDepartmentCommand(object p) => true;

    #endregion

    #region Constructor

    public AddDepartmentViewModel()
    {
        #region Commands

        AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);

        #endregion
        
    }

    #endregion
}