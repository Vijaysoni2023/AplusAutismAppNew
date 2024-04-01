using System.ComponentModel.DataAnnotations;

namespace aplusautism.Models
{
    public class LoginTracking
    {
        public long LoginTrackingID { get; set; }

        public int UserID { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        [MaxLength(50)]
        public string IPAddress { get; set; }

        [MaxLength(50)]
        public string DeviceID { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

    }
}
