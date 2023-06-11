using System.Collections.ObjectModel;

namespace BankApp.Models;

public class Department
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ObservableCollection<Client>? Clients { get; set; } = new();
}
