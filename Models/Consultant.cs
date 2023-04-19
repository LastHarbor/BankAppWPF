namespace BankApp.Models;

public class Consultant : User
{
    public Consultant() : base("Консультант", Role.Consultant, false){}
}