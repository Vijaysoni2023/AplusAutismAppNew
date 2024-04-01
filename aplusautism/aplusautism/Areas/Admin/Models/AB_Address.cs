using System.ComponentModel.DataAnnotations;

namespace aplusautism.Areas.Admin.Models
{
    public class AB_Address
    {
        public long AB_AddressID { get; set; }
       
        public long? AB_MainID { get; set; }

        [MaxLength(50)]
        public string AddressType { get; set; }

        [MaxLength(50)]
        public string RelationTableName { get; set; }


        public long? RelationTableID { get; set; }

        [MaxLength(50)]
        public string Address1 { get; set; }

        [MaxLength(50)]
        public string Address2 { get; set; }

       
        public int? CountryId { get; set; }

        public bool? IsStateExternal { get; set; }


        public string StateID { get; set; }

        public bool? IsCityExternal { get; set; }

        public string CityID { get; set; }

        [MaxLength(50)]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string ExternalValue1 { get; set; }

        [MaxLength(50)]
        public string ExternalValue2 { get; set; }

        [MaxLength(50)]
        public string ExternalValue3 { get; set; }

        [MaxLength(50)]
        public string ExternalValue4 { get; set; }

        public bool IsPrimary { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
