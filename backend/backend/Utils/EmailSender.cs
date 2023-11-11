using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace backend.Utils;
public class EmailSender
{
    private readonly string mail = "thinhdpham2510@gmail.com";
    private readonly string pw = "kipdrskqchpyqjdw";

    public EmailSender()
    {
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            string fromName = "FBlog Academy";
            string toEmail = email;

            using (var gmailClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(mail, pw),
                EnableSsl = true
            })
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(mail, fromName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(toEmail);

                await gmailClient.SendMailAsync(mailMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            // You might want to log the exception or take other appropriate actions.
        }
    }
}


