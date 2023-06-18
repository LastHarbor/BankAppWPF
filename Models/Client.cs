namespace BankApp.Models
{
    public sealed class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string MobileNumber { get; set; }
        public string PassportNumber { get; set;}
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
        
    }
}
