using System.ComponentModel.DataAnnotations;

namespace aplusautism.Data.Models
{
    public class GlobalCodes
    {
        [Key]
        public long GlobalCodeID { get; set; }

        [MaxLength(50)]
        public string CustomerSetupNumber { get; set; }

        [MaxLength(50)]
        public string GlobalCodeCategoryValue { get; set; }

        [MaxLength(50)]
        public string GlobalCodeValue { get; set; }

        [MaxLength(50)]
        public string GlobalCodeName { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string ExternalValue1 { get; set; }

        [MaxLength(50)]
        public string ExternalValue2 { get; set; }

        [MaxLength(50)]
        public string ExternalValue3 { get; set; }

        [MaxLength(50)]
        public string ExternalValue4 { get; set; }

        [MaxLength(50)]
        public string ExternalValue5 { get; set; }

        [MaxLength(50)]
        public string ExternalValue6 { get; set; }


        public bool IsActive { get; set; }

        public int? SortOrder { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool? IsFavorite { get; set; }

    }
}
