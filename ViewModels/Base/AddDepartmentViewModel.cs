using System.Windows.Input;

namespace BankApp.ViewModels.Base;

public class AddDepartmentViewModel : ViewModel
{
    private string _departmentName;

    public string DepartmentName
    {
        get => _departmentName;
        set => SetField(ref _departmentName, value);
    }

    #region AddDepartmentCommand


    public ICommand AddDepartmentCommand { get; }
    private void OnCommand(object p)
    {

    }
    private bool CanCommand(object p) => true;

    #endregion
}