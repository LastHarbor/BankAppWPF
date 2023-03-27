using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronimyc { get; set; }
        public string MobileNumer { get; set; }
        public string PassortNum { get; set;}
        public Department Department { get; set; }
    }
}
