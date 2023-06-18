using System;
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
        public static event EventHandler<User>? SelectedRoleChanged;

        private User? _selectedRole;

        public User SelectedRole
        {
            get => _selectedRole!;
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;
                    OnPropertyChanged();
                    SelectedRoleChanged?.Invoke(this, _selectedRole);
                }
            }
        }

        public ObservableCollection<User> Roles { get; set; } = new ()
        {
            new Manager(),
            new Consultant()
        };

        public ICommand? OpenWorkspaceWindowCommand { get; }
        private bool CanOpenWorkspaceWindowCommand(object p) => _selectedRole!=null;

        private void OnOpenWorkspaceWindowCommand(object p)
        {
            Extensions.Extensions.SetMainWindow(_workSpace);
        }   


        public MainWindowViewModel()
        {
            OpenWorkspaceWindowCommand = new LambdaCommand(OnOpenWorkspaceWindowCommand, CanOpenWorkspaceWindowCommand);
        }
    }
}


