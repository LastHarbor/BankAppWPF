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

        private readonly WorkspaceWindow _workSpace = new ();

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


        public ICommand? OpenWorkspaceWindowCommand { get; }
        private void OnOpenWorkspaceWindowCommand(object p)
        {
            Extensions.Extensions.SetMainWindow(_workSpace);
            Extensions.Extensions.CurrentUser = CurrentRole;
        }   
        private bool CanOpenWorkspaceWindowCommand(object p) => true;


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
            OpenWorkspaceWindowCommand = new LambdaCommand(OnOpenWorkspaceWindowCommand, CanOpenWorkspaceWindowCommand);
        }

        #endregion
        
    }
}


