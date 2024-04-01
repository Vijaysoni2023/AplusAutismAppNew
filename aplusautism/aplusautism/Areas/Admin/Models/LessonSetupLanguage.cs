namespace aplusautism.Areas.Admin.Models
{
    public class LessonSetupLanguage
    {
        public long LessonSetupLanguageID { get; set; }

        public long? LessonSetupID { get; set; }

        public string LanguageID { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
