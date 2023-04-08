using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Views.Windows;
using Microsoft.EntityFrameworkCore;

namespace BankApp.ViewModels.Base
{
    public class WorkspaceWindowViewModel : ViewModel
    {
        #region Collections

        public ObservableCollection<Department>? Departments => Singleton.GetInstance().GetDepartments();

        #endregion

        #region Singleton

        #endregion

        #region SelectedItem

        private Department _selectedDepartment = null!;
        public Department SelectedDepartment 
        { 
            get => _selectedDepartment; 
            set => SetField(ref _selectedDepartment, value);
        }
        private Client _selectedClient = null!;
        public Client SelectedClient 
        { 
            get => _selectedClient; 
            set => SetField(ref _selectedClient, value);
        }

        #endregion

        #region View

        private bool _isManager = false;
        public bool IsManager
        {
            get
            {
                if (Extensions.Extensions.CurrentUser!.Name == "Консультант")
                    return true;
                return false;
            }
            set => SetField(ref _isManager, value);
        }

        #endregion

        #region Commands


        public ICommand DeleteDepartmentCommand { get; }
        private void OnDeleteDepartmentCommand(object p)
        {
            using (var context = new DataContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // удаление департамента
                        context.Departments.Remove(SelectedDepartment);
                        context.SaveChanges();
                        SelectedDepartment.Clients.Clear();
                        Departments.Remove(SelectedDepartment);

                        // проверка на количество оставшихся департаментов
                        var remainingDepartments = context.Departments.Count();
                        if (remainingDepartments == 0)
                        {
                            // обнуление таблиц клиентов и департаментов
                            context.Database.ExecuteSqlRaw("DELETE FROM Clients");
                            context.Database.ExecuteSqlRaw("DELETE FROM Departments");
                            context.Database.ExecuteSqlRaw("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Clients'");
                            context.Database.ExecuteSqlRaw("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Departments'");
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Failed to delete department: {ex.Message}");
                    }
                    
                }
            }
        }
        private bool CanDeleteDepartmentCommand(object p) => true;

        #region ChangeUser


        public ICommand ChangeUserCommand { get; }
        private void OnChangeUserCommand(object p)
        {
            Extensions.Extensions.SetMainWindow(new MainWindow());
        }
        private bool CanChangeUserCommand(object p) => true;

        #endregion

        #region CheckUser

            public ICommand CheckUserCommand { get; }

            private void OnCheckUserCommand(object p)
            {
            switch (Extensions.Extensions.CurrentUser!.Name)
                {
                    case "Консультант":
                        MessageBox.Show("Вы вошли под консультантом");
                    break;

                    case "Менеджер":
                        MessageBox.Show("Вы вошли под менеджером");
                    break;
                }
            }

            private bool CanCheckUserCommand(object p) => true;
        #endregion

        #region AddDepartment

            public ICommand AddDepartmentCommand { get; }
            private void OnAddDepartmentCommand(object p)
            {
                Extensions.Extensions.ShowDialog(new AddDepartment());
            }
            private bool CanAddDepartmentCommand(object p) => true;

        #endregion

        #region AddClient


        public ICommand AddClientCommand { get; }
        private void OnAddClientCommand(object p)
        {
            Extensions.Extensions.ShowDialog(new AddClient());
        }
        private bool CanAddClientCommand(object p) => true;

        #endregion

        #endregion
        public WorkspaceWindowViewModel()
        {
            //Commands
            AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
            CheckUserCommand = new LambdaCommand(OnCheckUserCommand,CanCheckUserCommand);
            ChangeUserCommand = new LambdaCommand(OnChangeUserCommand, CanChangeUserCommand);
            AddClientCommand = new LambdaCommand(OnAddClientCommand, CanAddClientCommand);
            DeleteDepartmentCommand = new LambdaCommand(OnDeleteDepartmentCommand, CanDeleteDepartmentCommand);
            //Fields

        }

    }
}