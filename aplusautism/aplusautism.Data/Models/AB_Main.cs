using System.ComponentModel.DataAnnotations;

namespace aplusautism.Data.Models
{
    public class AB_Main
    {
        public long AB_MainID { get; set; }

        [MaxLength(50)]
        public string AB_Type { get; set; }

        public long? AB_MainIDParent { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string DeviceID { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string LegalIDType { get; set; }

        [MaxLength(50)]
        public string LegalID { get; set; }

        public DateTime? DOB { get; set; }

        public int? PreferedLanguage { get; set; }

        public bool? IsActive { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string ContactNumber { get; set; }

    }
}
