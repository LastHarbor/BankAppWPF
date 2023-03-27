using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Views.Windows;

namespace BankApp.ViewModels.Base
{
    public class MainWindowViewModel : ViewModel
    {
        #region Views

        private readonly Window _workSpace = new WorkspaceWindow();

        #endregion
        #region Roles

        public ObservableCollection<User> Roles { get; set; }

        private User? _currentUser;

        public User? CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        #endregion
        #region Commands

        public ICommand? OpenWorkspacewindowCommand { get; }
        private void OnOpenWorkspacewindowCommand(object p)
        {
            switch (CurrentUser!.Name)
            {
                case "Консультант":
                    _workSpace.DataContext = new WorkspaceWindowViewModel(CurrentUser);
                    Extensions.Extensions.SetMainWindow(_workSpace);
                    break;
                case "Менеджер":
                    _workSpace.DataContext = new WorkspaceWindowViewModel(CurrentUser);
                    Extensions.Extensions.SetMainWindow(_workSpace);
                    break;
            }
        }   
        private bool CanOpenWorkspacewindowCommand(object p) => true;

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
            CurrentUser = new User();
            //
            OpenWorkspacewindowCommand = new LambdaCommand(OnOpenWorkspacewindowCommand, CanOpenWorkspacewindowCommand);
        }

        #endregion
        
    }
}


