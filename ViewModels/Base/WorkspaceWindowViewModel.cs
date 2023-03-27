using System.Windows;
using System.Windows.Input;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Views.Windows;

namespace BankApp.ViewModels.Base
{
    public class WorkspaceWindowViewModel : ViewModel
    {
        #region DepartmentExtensions

        private Department.DepartmentExtensions _departmentExtensions;

        #endregion
        #region Role

        private User? _currentUser;

        public User? CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        #endregion
        #region Commands

        #region CheckUser

        public ICommand CheckUserCommand { get; }

        private void OnCheckUserCommand(object p)
        {
            if (CurrentUser!.Role is Role.Consultant)
            {
                MessageBox.Show("Вы вошли под консультантом");
            }
            else if (CurrentUser!.Role is Role.Manager)
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
            var addDepartmentViewModel = new AddDepartmentViewModel(_departmentExtensions);
            var addDepartment = new AddDepartment();
            addDepartment.ShowDialog();
        }
        private bool CanAddDepartmentCommand(object p) => true;

        #endregion

        #endregion
        public WorkspaceWindowViewModel(User? user, Department.DepartmentExtensions departmentExtensions)
        {
            AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
            CheckUserCommand = new LambdaCommand(OnCheckUserCommand,CanCheckUserCommand);
            //Role
            CurrentUser = user;
            _departmentExtensions = departmentExtensions;
            //
        }

    }
}