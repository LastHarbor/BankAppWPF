using System.Collections.ObjectModel;
using BankApp.Models;
using System.Windows.Input;
using BankApp.Extensions;

namespace BankApp.ViewModels.Base;

public class AddDepartmentViewModel : ViewModel
{
    #region DepartmentExtension

    private Department.DepartmentExtensions _departmentExtension = new Department.DepartmentExtensions();

    #endregion
    private string _departmentName;

    public string DepartmentName
    {
        get => _departmentName;
        set => SetField(ref _departmentName, value);
    }

    #region AddDepartmentCommand


    public ICommand AddDepartmentCommand { get; }
    private void OnAddDepartmentCommand(object p)
    {
        Department department = new()
        {
            Name = DepartmentName,
            Clients = new ObservableCollection<Client>()
        };
        _departmentExtension.AddDepartment(department);
        Extensions.Extensions.CloseWindow();
    }
    private bool CanAddDepartmentCommand(object p) => true;

    #endregion

    #region Constructor

    public AddDepartmentViewModel(Department.DepartmentExtensions departmentExtension)
    {
        #region Commands

        AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);

            #endregion
        _departmentExtension = departmentExtension;
    }

    #endregion
}