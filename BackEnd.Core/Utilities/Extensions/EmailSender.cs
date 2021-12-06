using System.Net.Mail;

namespace BackEnd.Core.Utilities.Extensions
{
    public class EmailSender
    {
        public static void Send(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("kpadashi6@gmail.com", "Digi shop");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential("kpadashi6@gmail.com", "Kasra@123456");
            smtpServer.EnableSsl = true;

            smtpServer.Send(mail);

        }
    }
}