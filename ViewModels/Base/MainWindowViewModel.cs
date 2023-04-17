using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BankApp.Data;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Views.Windows;

namespace BankApp.ViewModels.Base
{
    public class MainWindowViewModel : ViewModel
    {
       

        private readonly WorkspaceWindow _workSpace = new ();
        private User? _selectedRole;

        public User SelectedRole
        {
            get => _selectedRole!;
            set => SetField(ref _selectedRole, value);
        }
        public ObservableCollection<User> Roles { get; set; } = new ()
        {
            new Manager(),
            new Consultant()
        };

        public MainWindowViewModel()
        {
            OpenWorkspaceWindowCommand = new LambdaCommand(OnOpenWorkspaceWindowCommand, CanOpenWorkspaceWindowCommand);
        }

        public ICommand? OpenWorkspaceWindowCommand { get; }
        private void OnOpenWorkspaceWindowCommand(object p)
        {

            Extensions.Extensions.SetMainWindow(_workSpace);
            var workspacewindowviewmodel = new WorkspaceWindowViewModel(SelectedRole);
            _workSpace.DataContext = workspacewindowviewmodel;
        }
        private bool CanOpenWorkspaceWindowCommand(object p) => SelectedRole != null;
    }
}


