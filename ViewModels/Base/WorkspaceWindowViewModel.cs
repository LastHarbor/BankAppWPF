using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BankApp.Data;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Views.Windows;
using Microsoft.EntityFrameworkCore;

namespace BankApp.ViewModels.Base
{
    public class WorkspaceWindowViewModel : ViewModel
    {

        private readonly DataContext _context = new();
        private User? _currentUser;
        private ObservableCollection<Department> _departments = new();
        private ObservableCollection<Client> _clients = new();
        private int _countOfDepartments;
        private int _countOfClients;
        private Department _selectedDepartment = null!;
        private Client _selectedClient = null!;
        private  AddDepartment _addDepartment;
        private  AddClient _addClient;

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

        private User? CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser!, value);
        }

        public bool IsManager
        {
            get => CurrentUser!.IsEnabled;
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

        public ICommand DeleteDepartmentCommand { get; }
        public ICommand ChangeUserCommand { get; }
        public ICommand AddDepartmentCommand { get; }
        public ICommand CheckUserCommand { get; }
        public ICommand AddClientCommand { get; }
        public ICommand? TestDbCommand { get; }

        private bool CanDeleteDepartmentCommand(object p) => p is Department department && Departments!.Contains(department);
        private bool CanChangeUserCommand(object p) => true;
        private bool CanCheckUserCommand(object p) => true;
        private bool CanAddDepartmentCommand(object p) => true;
        private bool CanAddClientCommand(object p) => true;
        private bool CanTestDbCommand(object p) => true;


        public WorkspaceWindowViewModel(User currentUser)
        {
            LoadDb();
            this.CurrentUser = currentUser;
            //Commands
            AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
            CheckUserCommand = new LambdaCommand(OnCheckUserCommand,CanCheckUserCommand);
            ChangeUserCommand = new LambdaCommand(OnChangeUserCommand, CanChangeUserCommand);
            AddClientCommand = new LambdaCommand(OnAddClientCommand, CanAddClientCommand);
            DeleteDepartmentCommand = new LambdaCommand(OnDeleteDepartmentCommand, CanDeleteDepartmentCommand);
            
            //tests
            //TestDbCommand = new LambdaCommand(OnTestDbCommand, CanTestDbCommand);
        }

        private void Clients_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
           
        }

        private void OnDeleteDepartmentCommand(object p)
        {
            if (p is not Department department) return;
            _context.Departments.Remove(department);

            var depIndex = Departments!.IndexOf(department);
            Departments.Remove(department);

            if (depIndex < Departments.Count)
                SelectedDepartment = Departments[depIndex];

            if(!_context.Departments.Any()) CleareDatabase(_context);
            _context.SaveChangesAsync();
        }
        private void OnChangeUserCommand(object p)
        {
            Extensions.Extensions.SetMainWindow(new MainWindow());
            CurrentUser = null;
        }

        private void OnCheckUserCommand(object p)
        {
            if (CurrentUser is Consultant)
            {
                MessageBox.Show("Вы вошли под консультантом");
            }
            else if (CurrentUser is Manager)
            {
                MessageBox.Show("Вы вошли под менеджером");
            }
        }

        private void OnAddDepartmentCommand(object p)
        {
            _addDepartment = new ();
            _addDepartment.Closed += AddDepartment_Closed;
            Extensions.Extensions.ShowDialog(_addDepartment);
        }

        private void AddDepartment_Closed(object? sender, EventArgs e)
        {
            LoadDb();
        }

        private void OnAddClientCommand(object p)
        {
            _addClient = new();
            _addClient.Closed += AddClient_Closed;
            Extensions.Extensions.ShowDialog(new AddClient());
        }

        private void AddClient_Closed(object? sender, EventArgs e)
        {
            LoadDb();
        }

        private void CleareDatabase(DataContext dataContext)
        {
            dataContext.Database.ExecuteSqlRaw("DELETE FROM Departments");
            dataContext.Database.ExecuteSqlRaw("DELETE FROM Clients");

        }

        private void LoadDb()
        {
            Departments = new(_context.Departments.ToList()!);
            Departments.CollectionChanged += Departments_CollectionChanged!;

            CountOfDepartments = Departments.Count;
        }

        private void Departments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CountOfDepartments = Departments.Count;
        }
    }
}