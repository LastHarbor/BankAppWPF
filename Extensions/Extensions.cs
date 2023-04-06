using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BankApp.Models;

namespace BankApp.Extensions;

public static class Extensions
{
    public static User? CurrentUser { get; set; }

    public static void CloseWindow()
    {
        Window? window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }
    public static void CloseDialog()
    {
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        if (window != null)
        {
            window.DialogResult = true;
            window.Close();
        }
    }
    public static bool? ShowDialog(Window window)
    {
        if (window == null)
        {
            return false;
        }

        window.Owner = Application.Current.MainWindow;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        return window.ShowDialog();
    }

    public static void SetMainWindow(Window newWindow)
    {
        var mainWindow = Application.Current.MainWindow;
        Application.Current.MainWindow = newWindow;
        newWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        newWindow.Show();
        mainWindow?.Close(); 
    }


}

public abstract class Command : ICommand
{
    /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.</returns>
    public abstract bool CanExecute(object? parameter);

    /// <summary>Defines the method to be called when the command is invoked.</summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
    public abstract void Execute(object? parameter);

    /// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
