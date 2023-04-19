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
        private User _currentUser;
        private Department _selectedDepartment = null!;
        private Client _selectedClient = null!;
        private static readonly DataContext DataContext = new();
        private int _countOfDepartments;


        public User? CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        public bool IsManager => CurrentUser!.IsEnabled;
        
        public ObservableCollection<Department>? Departments => Singleton.GetInstance().GetDepartments();

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


        public WorkspaceWindowViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            Departments.CollectionChanged += Departments_CollectionChanged;
            CountOfDepartments = Departments?.Count ?? 0;
            //Commands
            AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
            CheckUserCommand = new LambdaCommand(OnCheckUserCommand, CanCheckUserCommand);
            ChangeUserCommand = new LambdaCommand(OnChangeUserCommand, CanChangeUserCommand);
            AddClientCommand = new LambdaCommand(OnAddClientCommand, CanAddClientCommand);
            DeleteDepartmentCommand = new LambdaCommand(OnDeleteDepartmentCommand, CanDeleteDepartmentCommand);

            //tests
            TestDbCommand = new LambdaCommand(OnTestDbCommand, CanTestDbCommand);
        }

        public ICommand DeleteDepartmentCommand { get; }
        private void OnDeleteDepartmentCommand(object p)
        {
            using (var transaction = DataContext.Database.BeginTransaction())
            {
                DataContext.Departments.Remove(SelectedDepartment);
                DataContext.SaveChanges();
                SelectedDepartment.Clients.Clear();
                Departments!.Remove(SelectedDepartment);
                if (!DataContext.Departments.Any())
                {
                   ClearDatabase(DataContext);
                }
                transaction.Commit();
            }
        }
        private bool CanDeleteDepartmentCommand(object p) => true;


        public ICommand ChangeUserCommand { get; }
        private void OnChangeUserCommand(object p)
        {
            Extensions.Extensions.SetMainWindow(new MainWindow());
            CurrentUser = null;
        }

        private bool CanChangeUserCommand(object p) => true;



        public ICommand CheckUserCommand { get; }
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

        private bool CanCheckUserCommand(object p) => true;


        public ICommand AddDepartmentCommand { get; }
        private void OnAddDepartmentCommand(object p)
        {
            Extensions.Extensions.ShowDialog(new AddDepartment());
        }

        private bool CanAddDepartmentCommand(object p) => true;


        public ICommand AddClientCommand { get; }
        private void OnAddClientCommand(object p)
        {
            Extensions.Extensions.ShowDialog(new AddClient());
        }

        private bool CanAddClientCommand(object p) => true;


        //Введите нужно число в цикле для создания нужного количества шиентов
        public ICommand TestDbCommand { get; }
        private void OnTestDbCommand(object p)
        {
            if (!DataContext.Departments.Any())
            {
                MessageBox.Show("Сначала создайте департамент");
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    var guid = Guid.NewGuid().ToString().Substring(0, 6);
                    var client = new Client
                    {
                        Name = guid,
                        Surname = guid,
                        Patronymic = guid,
                        MobileNumber = guid,
                        PassportNumber = guid,
                        DepartmentId = SelectedDepartment.Id
                    };
                    DataContext.Clients.Add(client);
                    SelectedDepartment.Clients!.Add(client);
                }
                DataContext.SaveChanges();
                MessageBox.Show("Succesfully added");
            }
        }
        private bool CanTestDbCommand(object p) => true;

        private void ClearDatabase(DataContext dataContext)
        {
            dataContext.Database.ExecuteSqlRaw(@"
            DELETE FROM Clients;
            DELETE FROM Departments;
            UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Clients';
            UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Departments';
            ");
        }

        private void Departments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CountOfDepartments = Departments?.Count ?? 0;
        }
    }
}