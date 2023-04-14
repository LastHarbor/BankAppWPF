namespace BankApp.Models;

public enum Role
{
    Manager,
    Consultant
}

public abstract class User
{
    public string Name { get; set; }
    public Role Role { get; }
    public bool IsEnabled { get; set; }

    protected User(string name, Role role, bool isEnabled)
    {
        Name = name;
        Role = role;
        IsEnabled = isEnabled;
    }
}