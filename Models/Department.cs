using System.Collections.ObjectModel;
using System.Linq;
using BankApp.Data;
using BankApp.ViewModels.Base;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Models;

public class Department
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ObservableCollection<Client>? Clients { get; set; } = new();

}
