namespace SurfLevel.Domain.Options
{
    public class SMTPOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public Credentials Credentials { get; set; }
    }

    public class Credentials
    {
        public string From { get; set; }
        public string Password { get; set; }
    }
}
