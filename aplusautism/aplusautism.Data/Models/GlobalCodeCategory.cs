using System.ComponentModel.DataAnnotations;

namespace aplusautism.Data.Models
{ 
    public class GlobalCodeCategory
    {

        public int GlobalCodeCategoryID { get; set; }
        [MaxLength(50)]
        public string CustomerSetupNumber { get; set; }
        [MaxLength(50)]
        public string GlobalCodeCategoryValue { get; set; }
        [MaxLength(50)]
        public string GlobalCodeCategoryName { get; set; }
        [MaxLength(50)]
        public bool IsActive { get; set; }
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
        public int? SortOrder { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

    }
}
