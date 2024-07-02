namespace DATN.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(string email, string subject, string message);
        void Dispose();
        void Connect();
    }
}
