﻿using System;
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
        #region Fields

        private User _currentUser;
        public User? CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        public bool IsManager
        {
            get => CurrentUser!.IsEnabled;
        }
        #endregion

        #region Collections

        public ObservableCollection<Department>? Departments => Singleton.GetInstance().GetDepartments();

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

        
        public WorkspaceWindowViewModel(User currentUser)
        {
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
                        SelectedDepartment.Clients!.Clear();
                        Departments!.Remove(SelectedDepartment);


                        int remainingDepartments = context.Departments.Count();
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
            CurrentUser = null;
        }
        private bool CanChangeUserCommand(object p) => true;

        #endregion

        #region CheckUser

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
        //public ICommand TestDbCommand { get; }
        //private void OnTestDbCommand(object p)
        //{
        //    //using (var context = new DataContext())
        //    //{
        //    //    var department = new Department { Name = "IT Department", Id = 1};
        //    //    var client = new Client
        //    //    {
        //    //        Name = "John",
        //    //        Surname = "Doe",
        //    //        Patronimyc = "Smith",
        //    //        MobileNumber = "+123456789",
        //    //        PassportNumber = "1234567890",
        //    //        DepartmentId = department.Id
        //    //    };
        //    //    context.Departments.Add(department);
        //    //    context.Clients.Add(client);
        //    //    context.SaveChanges();
        //    //    MessageBox.Show("Succesfully added");
        //    //}
        //}
        //private bool CanTestDbCommand(object p) => true;

        #endregion

        #endregion
    }
}