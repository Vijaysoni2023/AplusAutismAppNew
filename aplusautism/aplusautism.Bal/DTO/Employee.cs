using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace aplusautism.Bal.DTO
{
    public class Employee
    {
        [Display(Name = "Serial No")]
        public byte EmployeeId { get; set; }

        [Display(Name = "Name")]
        public string EmployeeName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
