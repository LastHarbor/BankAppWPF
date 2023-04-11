using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronimyc { get; set; }
        public string MobileNumber { get; set; }
        public string PassportNumber { get; set;}
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        
    }
}
