using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BankApp.Data;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BankApp.ViewModels.Base;

public class WorkspaceWindowViewModel : ViewModel
{

        private readonly DataContext _context = new();
        private User? _currentUser;
        private ObservableCollection<Department> _departments = new();
        private ObservableCollection<Client> _clients = new();
        private Department _selectedDepartment = null!;
        private Client _selectedClient = null!;
        private AddDepartment _addDepartment;
        private AddClient _addClient;
        private int _countOfDepartments;
        private int _countOfClients;

        public ObservableCollection<Department>? Departments
        {
            get => _departments;
            set => SetField(ref _departments, value);
        }

        public ObservableCollection<Client>? Clients
        {
            get => _clients;
            set => SetField(ref _clients, value);
        }

        public User? CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        public bool IsManager
        {
            get => CurrentUser?.IsEnabled ?? false ;
        }
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set => SetField(ref _selectedDepartment, value);
        }
        public Client SelectedClient
        {
            get => _selectedClient;
            set => SetField(ref _selectedClient, value);
        }

        public int CountOfDepartments
        {
            get => _countOfDepartments;
            set => SetField(ref _countOfDepartments, value);
        }

        public int CountOfClients
        {
            get => _countOfClients;
            set => SetField(ref _countOfClients, value);
        }

        private void OnClearClientsCommand(object p)
        {
            ClearClients();
        }

        public ICommand DeleteCommand { get; }
        public ICommand ChangeUserCommand { get; }
        public ICommand AddDepartmentCommand { get; }
        public ICommand CheckUserCommand { get; }
        public ICommand AddClientCommand { get; }
        public ICommand TestDbCommand { get; }
        public ICommand ClearDatabaseCommand { get; }
        public ICommand ClearClientsCommand { get; }

        private bool CanChangeUserCommand(object p) => true;
        private bool CanCheckUserCommand(object p) => true;
        private bool CanAddDepartmentCommand(object p) => true;
        private bool CanAddClientCommand(object p) => SelectedDepartment != null;
        private bool CanTestDbCommand(object p) => true;
        private bool CanClearDatabaseCommand(object p) => true;
        private bool CanClearClientsCommand(object p) => SelectedDepartment !=null;
        private bool CanDeleteCommand(object p)
        {
            if (p is Department department)
            {
                return Departments?.Contains(department) == true;
            }
            else if (p is Client client)
            {
                return Clients?.Contains(client) == true;
            }

            return false;
        }



        public WorkspaceWindowViewModel()
        {
            MainWindowViewModel.SelectedRoleChanged += MainWindowViewModel_SelectedRoleChanged!;
            //Commands
            AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
            ChangeUserCommand = new LambdaCommand(OnChangeUserCommand, CanChangeUserCommand);
            AddClientCommand = new LambdaCommand(OnAddClientCommand, CanAddClientCommand);
            DeleteCommand = new LambdaCommand(OnDeleteCommand, CanDeleteCommand);
            ClearDatabaseCommand = new LambdaCommand(OnClearDatabaseCommand, CanClearDatabaseCommand);
            ClearClientsCommand = new LambdaCommand(OnClearClientsCommand, CanClearClientsCommand);
            //tests
            //TestDbCommand = new LambdaCommand(OnTestDbCommand, CanTestDbCommand);
        }

       
        private void OnChangeUserCommand(object p)
        {
            Extensions.Extensions.SetMainWindow(new MainWindow());
            CurrentUser = null;
        }


        private void OnAddDepartmentCommand(object p)
        {
            _addDepartment = new ();
            var addDepartmentViewModel = new AddDepartmentViewModel { Title = "Добавить" };
            _addDepartment.DataContext = addDepartmentViewModel;
            _addDepartment.Closed += AddDepartment_Closed;
            Extensions.Extensions.ShowDialog(_addDepartment);
        }

        private void OnAddClientCommand(object p)
        {
            _addClient = new AddClient();
            _addClient.Closed += AddClient_Closed;
            var addClientViewModel = new AddClientViewModel { SelectedDepartment = SelectedDepartment, Title = "Добавить"};
            _addClient.DataContext = addClientViewModel;
            Extensions.Extensions.ShowDialog(_addClient);
        }
        private async void OnClearDatabaseCommand(object p)
        {
           await CleareDatabase(_context);
        }

        private async Task CleareDatabase(DataContext dataContext)
        {
            Departments!.Clear();
            Clients!.Clear();
            CountOfClients = 0;
            CountOfDepartments = 0;
            dataContext.Database.ExecuteSqlRaw("DELETE FROM Departments");
            dataContext.Database.ExecuteSqlRaw("DELETE FROM Clients");
            dataContext.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name IN ('Departments', 'Clients')");
            await dataContext.SaveChangesAsync();
        }

        private async Task LoadDb()
        {
            Departments = new( await _context.Departments.ToListAsync()!);
            Departments.CollectionChanged += Departments_CollectionChanged!;
            CountOfDepartments = Departments.Count;

            Clients = new(await _context.Clients.ToListAsync()!);
            Clients.CollectionChanged += Clients_CollectionChanged;
            CountOfClients = Clients.Count;
        }

        private async void OnDeleteCommand(object p)
        {
            Delete(p);

            if (!Departments!.Any())
            {
                await CleareDatabase(_context);
            }
        }

        private void Delete<T>(T selectedObject)
        {
            switch (selectedObject)
            {
                case Department department:
                {
                    ClearClients();

                    _context.Departments.Remove(department);
                    var depIndex = Departments!.IndexOf(department);
                    Departments.Remove(department);

                    if (depIndex < Departments.Count)
                        SelectedDepartment = Departments[depIndex];
                    else if (Departments.Any())
                        SelectedDepartment = Departments[^1];
                    if (selectedObject is Department)
                    {
                        _context.Database.ExecuteSqlRaw($"UPDATE sqlite_sequence SET seq = {Departments.Count} WHERE name = 'Departments'");
                    }

                    _context.SaveChangesAsync().ContinueWith(_ => LoadDb());

                    break;
                }
                case Client client:
                {
                    _context.Clients.Remove(client);
                    var clIndex = Clients!.IndexOf(client);
                    Clients.Remove(client);

                    if (clIndex < Clients.Count)
                        SelectedClient = Clients[clIndex];
                    else if (Clients.Any())
                        SelectedClient = Clients[^1];

                    _context.SaveChangesAsync();

                    if (!Clients.Any())
                        ClearClients();

                    if (selectedObject is Client)
                    {
                        _context.Database.ExecuteSqlRaw($"UPDATE sqlite_sequence SET seq = {Clients.Count} WHERE name = 'Clients'");
                    } 

                    break;
                }
            }
        }

        private async void ClearClients()
        {
            if (Departments!.Count == 1)
            {
                _context.Clients.RemoveRange(_context.Clients);
                _context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Clients'");
                await _context.SaveChangesAsync().ContinueWith(_ => LoadDb());
            }
            else if (SelectedDepartment != null)
            {
                var clientsToRemove = Clients.Where(c => c.DepartmentId == SelectedDepartment.Id).ToList();
                foreach (var client in clientsToRemove)
                {
                    _context.Clients.Remove(client);
                    Clients.Remove(client);
                }
                //_context.Database.ExecuteSqlRaw($"UPDATE sqlite_sequence SET seq = {Clients.Count} WHERE name = 'Clients'");

            await _context.SaveChangesAsync().ContinueWith(_ => LoadDb());
            }
            
        }

        #region Events
        private void Departments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CountOfDepartments = Departments!.Count;
        }
        private async void AddDepartment_Closed(object? sender, EventArgs e)
        {
           await LoadDb();
        }
        private void Clients_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CountOfClients = Clients.Count;
        }
        private async void MainWindowViewModel_SelectedRoleChanged(object sender, User selectedRole)
        {
            CurrentUser = selectedRole;
            OnPropertyChanged(nameof(IsManager));
            await LoadDb();
    }

        private async void AddClient_Closed(object? sender, EventArgs e)
        {
            await LoadDb();
        }
        #endregion
}
