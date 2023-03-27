namespace BankApp.Models;

public enum Role
{
    Manager,
    Consultant
}

public class User
{
    public string? Name { get; set; }
    public Role Role { get; set; }
}