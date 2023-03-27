using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BankApp.Extensions;

public class Extensions
{
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

    public static void SetMainWindow(Window window)
    {
        Application.Current.MainWindow?.Close();
        Application.Current.MainWindow = window;
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.Show();
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
