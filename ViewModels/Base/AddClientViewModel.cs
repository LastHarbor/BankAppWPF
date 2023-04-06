using System.Collections.ObjectModel;
using System.Windows.Input;
using BankApp.Extensions;
using BankApp.Models;

namespace BankApp.ViewModels.Base;

public class AddClientViewModel : ViewModel
{
    #region Fields

    private Department _selectedDepartment;
    public Department SelectedDepartment
    {
        get => _selectedDepartment;
        set => SetField(ref _selectedDepartment, value);
    }

    #endregion
    #region Collections

    public ObservableCollection<Department> Departments
    {
        get => Singleton.GetInstance().GetDepartments();
    }
    #endregion

    #region Client
    
    private int _clientId;
    public int ClientId
    {
        get
        {
            _clientId = SelectedDepartment.Clients.Count;
            _clientId++;
            return _clientId;
        }
        set => SetField(ref _clientId, value);
    }
    private string _name;
    public string Name
    {
        get => _name; 
        set => SetField(ref _name, value);
    }

    private string _surname;
    public string Surname
    {
        get => _surname; 
        set => SetField(ref _surname, value);
    }

    private string _patronimyc;
    public string Patronimyc 
    { 
        get => _patronimyc; 
        set=> SetField(ref _patronimyc, value);
    }

    private string _mobileNumber;

    public string MobileNumer
    {
        get => _mobileNumber; 
        set => SetField(ref _mobileNumber, value);
    }

    private string _passportNum;
    public string PassortNum
    {
        get => _passportNum; 
        set=> SetField(ref _passportNum, value);
    }

    #endregion

    #region Commands

    public ICommand AddClientCommand { get; }
    private void OnAddClientCommand(object p)
    {
        SelectedDepartment.Clients!.Add(new Client()
        {
            Name = Name,
            Surname = Surname,
            Patronimyc = Patronimyc,
            MobileNumber = MobileNumer,
            PassportNumber = PassortNum,
            Department = SelectedDepartment,
            ClientId = ClientId
        });
        Extensions.Extensions.CloseDialog();
    }
    private bool CanAddClientCommand(object p) => true;       

    #endregion
    public AddClientViewModel()
    {
        AddClientCommand = new LambdaCommand(OnAddClientCommand, CanAddClientCommand);
    }

    
}