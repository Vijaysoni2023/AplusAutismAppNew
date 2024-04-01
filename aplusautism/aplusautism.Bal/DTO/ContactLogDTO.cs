namespace aplusautism.Bal.DTO
{
    public class ContactLogDTO
    {
        public long ClientContactLogID { get; set; }

        public long? AB_MAINID { get; set; }

        public string ContactType { get; set; }

        public string ContactTypeTopic { get; set; }

        public string ContactMethod { get; set; }

        public DateTime? FollowUpDate { get; set; }

        public long? FollowUpType { get; set; }

        public DateTime? FollowUpCompletedOn { get; set; }

        public long? FollowUpCompletedBy { get; set; }

        public string Subject { get; set; }

        public string Disposition { get; set; }

        public bool? FollowUpCompleted { get; set; }

        public string CompletionNotes { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string LogFromModule { get; set; }

        public bool? IsRead { get; set; }

        public string ScreenID { get; set; }

        public string ContactLogAttributeValue { get; set; }

        public string ContactLogAttributeRelation { get; set; }

        public long? VerifiedBy { get; set; }

        public DateTime? VerifiedOn { get; set; }

        public string VerifiedNotes { get; set; }

        public long? Isverified { get; set; }

        public string VerificationStatus { get; set; }

        public string RelationAttribute { get; set; }

        public string RelationValue { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
