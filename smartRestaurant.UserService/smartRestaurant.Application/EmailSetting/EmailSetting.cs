
namespace smartRestaurant.Application.EmailSettings
{
    public class EmailSetting
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public string SenderName { get; set; } = default!;
        public string SenderEmail { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool EnableSsl { get; set; }
    }
}
