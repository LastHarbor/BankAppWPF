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
        #region ViewModels and Views
        //Viewmodels
        //Views
            private readonly AddDepartment _addDepartment;
        #endregion
        #region Collections

        private ObservableCollection<Department>? _departments;

        public ObservableCollection<Department>? Departments
        {
            get => _departments;
            set => SetField(ref _departments, value);
        }

        #endregion
        #region Singleton

        #endregion
        #region Role

        
        #endregion
        #region Commands

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
                _addDepartment.ShowDialog();
            }
            private bool CanAddDepartmentCommand(object p) => true;

        #endregion

        #endregion
            public WorkspaceWindowViewModel()
            {
                AddDepartmentCommand = new LambdaCommand(OnAddDepartmentCommand, CanAddDepartmentCommand);
                CheckUserCommand = new LambdaCommand(OnCheckUserCommand,CanCheckUserCommand);
                
            }

    }
}