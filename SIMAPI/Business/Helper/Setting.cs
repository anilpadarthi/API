namespace SIMAPI.Business.Helper
{
    public class MailSettings
    {
        public string fromMail { get; set; }
        public string fromPassword { get; set; }
        public string smtpHost { get; set; }
        public string smtpPort { get; set; }
        public string useSSL { get; set; }
        public string IsTest { get; set; }
        public string testMail { get; set; }
    }
    public class ConnectionStrings
    {
        public string DevConnection { get; set; }
    }
}
