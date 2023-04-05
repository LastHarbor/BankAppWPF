using System.Collections.ObjectModel;
using BankApp.Models;
using System.Windows.Input;
using BankApp.Extensions;

namespace BankApp.ViewModels.Base;

public class AddDepartmentViewModel : ViewModel
{
    #region DepartmentExtension

    #endregion
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
        Department department = new()
        {
            Name = DepartmentName,
            Clients = new ObservableCollection<Client>()
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