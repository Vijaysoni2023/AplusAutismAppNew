namespace aplusautism.Bal.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public string DeviceId { get; set; }

        public string IpAddress { get; set; }
    }
}
