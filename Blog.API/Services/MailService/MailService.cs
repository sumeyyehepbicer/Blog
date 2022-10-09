using Blog.Shared.Models.RequestModels;
using Blog.Shared.Utils;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Blog.API.Services.MailService
{
    public class MailService: IMailService
    {
        MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmail(EmailRequest req)
        {
            try
            {
                var _defaultCredentials = false;
                var _enableSsl = true;
                using (var smtpClient = new SmtpClient(_mailSettings.SmtpHost, _mailSettings.SmtpPort))
                {
                    smtpClient.EnableSsl = _enableSsl;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = _defaultCredentials;
                    if (_defaultCredentials == false)
                    {
                        smtpClient.Credentials = new NetworkCredential(_mailSettings.SmtpUser, _mailSettings.SmtpPassword);
                    }
                    MailAddress from = new MailAddress(_mailSettings.EmailFrom);
                    MailAddress to = new MailAddress(req.To);

                    MailMessage message = new MailMessage(from, to);
                 

                    message.Body = req.Body;
                    message.Subject = req.Subject;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;

                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
