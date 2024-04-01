using System.ComponentModel.DataAnnotations;

namespace aplusautism.Data.Models
{
    public class Payments
    {
        public int Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public long? AB_UserID { get; set; }

        public long? AB_MainID { get; set; }
        

        [Required(ErrorMessage = "Card Number is required")]
        [MaxLength(20)]
        [Display(Name = "Card Number")]
        public string CardNumder { get; set; }

       
        [Display(Name = "Stripe Customer ID")]
        public string StripeCustomerID { get; set; }

        [MaxLength(100)]
        public string StripeCardID { get; set; }
        
        public string Email { get; set; }

        public string CustomerName { get; set; }
        

        public DateTime? CreatedDate { get; set; }

        public long? Createdby { get; set; }

        public long? SubscriptionSetupID { get; set; }
        public bool? IsActive { get; set; }

        public bool? PaymentStatus { get; set; }
        
    }
}
