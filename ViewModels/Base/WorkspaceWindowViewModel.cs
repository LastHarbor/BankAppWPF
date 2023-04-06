using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Views.Windows;

namespace BankApp.ViewModels.Base
{
    public class WorkspaceWindowViewModel : ViewModel
    {
        #region Collections

        public ObservableCollection<Department>? Departments
        {
            get => Singleton.GetInstance().GetDepartments();
        }

        #endregion

        #region Singleton

        #endregion

        #region SelectedItem

        private Department _selectedDepartment;
        public Department SelectedDepartment 
        { 
            get => _selectedDepartment; 
            set => SetField(ref _selectedDepartment, value);
        }

        #endregion

        #region View

        private bool _isManager = false;

        public bool IsManager
        {
            get
            {
                if (Extensions.Extensions.CurrentUser.Name == "Консультант")
                    return true;
                return false;
            }
        }

        #endregion
        
        #region Commands

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
            switch (Extensions.Extensions.CurrentUser.Name)
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
            //Fields

        }

    }
}