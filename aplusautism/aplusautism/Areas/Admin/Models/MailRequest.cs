namespace aplusautism.Areas.Admin.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }

    }

    public class Emailmodel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    //public class Emailmodel
    //{
    //    public string From { get; set; }
    //    public string To { get; set; }
    //    public string Subject { get; set; }
    //    public string Body { get; set; }
    //}

}
