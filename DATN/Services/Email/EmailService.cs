using DATN.Services.Email;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;

    public EmailService()
    {
        _smtpClient = new SmtpClient();
        
    }

    public void SendEmail(string email, string subject, string message)
    {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("siliencer1606@gmail.com"));
            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = subject;
            mail.Body = new TextPart(TextFormat.Text) { Text = message };

            _smtpClient.Send(mail);      
    }

    public void Connect()
    {
        _smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        _smtpClient.Authenticate("siliencer1606@gmail.com", "yncf pxxy cjic xnbc");
    }

    public void Dispose()
    {
        _smtpClient.Disconnect(true);
        _smtpClient.Dispose();
    }
}
