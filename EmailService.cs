using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;


    public class EmailService
    {
        string senderName = "Омутных Олег";
        string senderMailAddress = "omutnih1234@gmail.com";


        public async Task SendEmailAsync(string email, string subject, string message, string getterName)
        {
            using var emailMessage = new MimeMessage();
 
            emailMessage.From.Add(new MailboxAddress(senderName, senderMailAddress));
            emailMessage.To.Add(new MailboxAddress(getterName, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            //emailMessage.Attachments.Append();
             
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, false);
                await client.AuthenticateAsync("omutnih1234@gmail.com", "password");
                await client.SendAsync(emailMessage);
 
                await client.DisconnectAsync(true);
            }
        }
    }
