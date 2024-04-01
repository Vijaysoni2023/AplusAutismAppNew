namespace aplusautism.Bal.DTO
{
    public class DeviceTrackingDTO
    {
        public int id { get; set; }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }
        public string IpAddress { get; set; }
        

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
