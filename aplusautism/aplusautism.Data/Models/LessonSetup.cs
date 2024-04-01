namespace aplusautism.Data.Models
{
    public class LessonSetup
    {
        public long LessonSetupID { get; set; }

        public string LessonTitle { get; set; }
      

        public string LessonDescription { get; set; }

        public string LessonVideoPath { get; set; }

        public string LessonTrailVideoPath { get; set; }

 
        public string LessonLanguage { get; set; }
        public string LessonStatus { get; set; }

        public string ExtraField1 { get; set; }

        public string ExtraField2 { get; set; }

        public string ExtraField3 { get; set; }

        public string ExtraField4 { get; set; }

        public string ExtraValue1Relation { get; set; }

        public string ExtraValue1Value { get; set; }

        public string ExtraValue2Relation { get; set; }

        public string ExtraValue2Value { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public long? DeletedBy { get; set; }

        public int? LessonCategoryId { get; set; }
        public string MobileVideoPath { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int SortOrder { get; set; }

        public bool IsFavorite { get; set; }
    }
}
