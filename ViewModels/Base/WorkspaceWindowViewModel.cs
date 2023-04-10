using System;
using System.Collections.ObjectModel;
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

        #region DeleteDepartment

        public ICommand DeleteDepartmentCommand { get; }
        private void OnDeleteDepartmentCommand(object p)
        {
            using (var context = new DataContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        context.Departments.Remove(SelectedDepartment);
                        context.SaveChanges();
                        SelectedDepartment.Clients.Clear();
                        Departments.Remove(SelectedDepartment);


                        var remainingDepartments = context.Departments.Count();
                        if (remainingDepartments == 0)
                        {

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


        #endregion

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

        #region DatabaseTest


        public ICommand TestDbCommand { get; }
        private void OnTestDbCommand(object p)
        {
            //using (var context = new DataContext())
            //{
            //    var department = new Department { Name = "IT Department", Id = 1};
            //    var client = new Client
            //    {
            //        Name = "John",
            //        Surname = "Doe",
            //        Patronimyc = "Smith",
            //        MobileNumber = "+123456789",
            //        PassportNumber = "1234567890",
            //        DepartmentId = department.Id
            //    };
            //    context.Departments.Add(department);
            //    context.Clients.Add(client);
            //    context.SaveChanges();
            //    MessageBox.Show("Succesfully added");
            //}
        }
        private bool CanTestDbCommand(object p) => true;

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
            //tests
            TestDbCommand = new LambdaCommand(OnTestDbCommand, CanTestDbCommand);
        }

    }
}