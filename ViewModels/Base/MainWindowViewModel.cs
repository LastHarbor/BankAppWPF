using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Views.Windows;

namespace BankApp.ViewModels.Base
{
    public class MainWindowViewModel : ViewModel
    {
        #region Collectiions


        #endregion
        #region Views and ViewModel

        private readonly WorkspaceWindow _workSpace = new WorkspaceWindow();

        #endregion
        #region Roles

        private User _currentRole;
        public User CurrentRole
        {
            get => _currentRole;
            set => SetField(ref _currentRole, value);
        }

        public ObservableCollection<User> Roles { get; set; }


        #endregion
        #region Commands


        public ICommand? OpenWorkspacewindowCommand { get; }
        private void OnOpenWorkspacewindowCommand(object p)
        {
            Extensions.Extensions.SetMainWindow(_workSpace);
            Extensions.Extensions.CurrentUser = CurrentRole;
        }   
        private bool CanOpenWorkspacewindowCommand(object p) => Roles.Any();


        #endregion
        #region Constructor

        public MainWindowViewModel()
        {
            //UserRoles
            Roles = new ObservableCollection<User>
            {
                new User() { Name = "Консультант", Role = Role.Consultant },
                new User() { Name = "Менеджер", Role = Role.Manager }
            };
            

            //
            OpenWorkspacewindowCommand = new LambdaCommand(OnOpenWorkspacewindowCommand, CanOpenWorkspacewindowCommand);
        }

        #endregion
        
    }
}


