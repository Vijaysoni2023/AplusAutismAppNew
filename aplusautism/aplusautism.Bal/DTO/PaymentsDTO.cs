using aplusautism.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class PaymentsDTO : Payments
    {
        [Required(ErrorMessage = "Expiration Year is required")]
        [Display(Name = "Expiration Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Expiration Month is required")]
        [Display(Name = "Expiration Month")]
        public int Month { get; set; }

        public string CVC { get; set; }

        public string AmountPaid { get; set; }

        public string SchemeName { get; set; }

        public string UserName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PayDate { get; set; }

        public string SubDescription { get; set; }


    }
}
