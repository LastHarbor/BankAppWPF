using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BankApp.Data;
using BankApp.Extensions;
using BankApp.Models;

namespace BankApp.ViewModels.Base;

public class AddClientViewModel : ViewModel
{
    private DataContext _context;
    private Department _selectedDepartment;
    private int _depId;


    public Department SelectedDepartment
    {
        get => _selectedDepartment;
        set => SetField(ref _selectedDepartment, value);
    }
    public int DepId
    {
        get => SelectedDepartment.Id;
        set => _depId = value;
    }


    #region Client

    private int _clientId;

    public int ClientId
    {
        get => _clientId;
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

    private string _patronymic;

    public string Patronymic
    {
        get => _patronymic;
        set => SetField(ref _patronymic, value);
    }

    private string _mobileNumber;

    public string MobileNumber
    {
        get => _mobileNumber;
        set => SetField(ref _mobileNumber, value);
    }

    private string _passportNum;

    public string PassportNum
    {
        get => _passportNum;
        set => SetField(ref _passportNum, value);
    }

    #endregion

    public ICommand AddClientCommand { get; }

    private void OnAddClientCommand(object p)
    {
        Client newClient = new Client()
        {
            ClientId = ClientId,
            Name = Name,
            Surname = Surname,
            Patronymic = Patronymic,
            MobileNumber = MobileNumber,
            PassportNumber = PassportNum,
            DepartmentId = DepId,
            
        };
        using (_context = new DataContext())
        {
            _context.Clients.Add(newClient);
            _context.SaveChangesAsync();
        }
        Extensions.Extensions.CloseDialog();
    }

    private bool CanAddClientCommand(object p) => true;

   

    public AddClientViewModel()
    {
        AddClientCommand = new LambdaCommand(OnAddClientCommand, CanAddClientCommand);
    }
}