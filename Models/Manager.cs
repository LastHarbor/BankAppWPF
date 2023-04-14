using System.Xml.Linq;

namespace BankApp.Models;

public class Manager : User
{
    public Manager() : base("Менеджер", Role.Manager, true) { }
}