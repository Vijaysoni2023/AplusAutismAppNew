using System.ComponentModel.DataAnnotations;

namespace aplusautism.Areas.Admin.Models
{
    public class SubscriptionSetup
    {
        public long SubscriptionSetupID { get; set; }

        [MaxLength(50)]
        public string SchemeName { get; set; }

        [MaxLength(50)]
        public string SubscriptionStatus { get; set; }

        public DateTime? ActivationStart { get; set; }

        public DateTime? ActivationEnd { get; set; }

        [MaxLength(50)]
        public string DurationType { get; set; }

        [MaxLength(50)]
        public int? Duration { get; set; }

        public decimal? Price { get; set; }

        [MaxLength(50)]
        public string UserNotes { get; set; }

        [MaxLength(50)]
        public string ExtraField1 { get; set; }

        [MaxLength(50)]
        public string ExtraField2 { get; set; }

        [MaxLength(50)]
        public string ExtraField3 { get; set; }

        [MaxLength(50)]
        public string ExtraField4 { get; set; }

        [MaxLength(50)]
        public string ExtraValue1Relation { get; set; }

        [MaxLength(50)]
        public string ExtraValue1Value { get; set; }

        [MaxLength(50)]
        public string ExtraValue2Relation { get; set; }

        [MaxLength(50)]
        public string ExtraValue2Value { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

    }
}
