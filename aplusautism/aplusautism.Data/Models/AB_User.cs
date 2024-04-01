using System.ComponentModel.DataAnnotations;

namespace aplusautism.Data.Models
{
    public class AB_User
    {
        public long AB_UserID { get; set; }

        public long? AB_MainID { get; set; }

        public string? SecurityQuestionID { get; set; }

        [MaxLength(50)]
        public string SecurityAnswer { get; set; }

        [MaxLength(50)]
        public string UserGroup { get; set; }

        [MaxLength(50)]
        public string? UserType { get; set; }

        public Guid? MemberShip_UserID { get; set; }

        public string UserName { get; set; }

        [MaxLength(50)]
        public string? Password { get; set; }

        public bool? IsFirstLogin { get; set; }

        public bool? IsActive { get; set; }

        public int? FailedLoginAttempts { get; set; }

        public DateTime? LastInvalidLogin { get; set; }

        public DateTime? LastResetPassword { get; set; }

        public DateTime? LastLogin { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string? oldstatus { get; set; }

    }
}
