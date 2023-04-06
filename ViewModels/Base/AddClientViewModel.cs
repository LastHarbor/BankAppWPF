using System.Collections.ObjectModel;
using BankApp.Models;

namespace BankApp.ViewModels.Base;

public class AddClientViewModel
{
    #region Collections

    public ObservableCollection<Department> Departments
    {
        get => Singleton.GetInstance().GetDepartments();
    }

    #endregion
}