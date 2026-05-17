namespace ASP.NET_IDENTITY.Settings
{
    public class SmtpSetting
    {
        
        public required string Host { get; set; }
        public int Port { get; set; }
        public required string User { get; set; }
        public required string Password { get; set; }
    }
}
