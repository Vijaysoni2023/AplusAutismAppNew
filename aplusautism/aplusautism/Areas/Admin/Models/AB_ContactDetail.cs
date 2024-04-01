using System.ComponentModel.DataAnnotations;

namespace aplusautism.Areas.Admin.Models
{
    public class AB_ContactDetail
    {
        public long AB_ContactDetailID { get; set; }

        public long? AB_MainID { get; set; }

        [MaxLength(50)]
        public string PhoneType { get; set; }

        [MaxLength(50)]
        public string PhoneCountryCode { get; set; }

        [MaxLength(50)]
        public string PhoneNo { get; set; }

        public bool IsPrimary { get; set; }

        public bool? IsdontContact { get; set; }

        public bool? IsVerified { get; set; }

        public DateTime? VerifyOn { get; set; }

        public long? VerifyBy { get; set; }

        [MaxLength(50)]
        public string StatusDescription { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
