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

        #endregion
        public WorkspaceWindowViewModel()
        {
            //Commands
            AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
            CheckUserCommand = new LambdaCommand(OnCheckUserCommand,CanCheckUserCommand);
            ChangeUserCommand = new LambdaCommand(OnChangeUserCommand, CanChangeUserCommand);
            //Fields
            
        }

    }
}