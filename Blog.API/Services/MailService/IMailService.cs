using Blog.Shared.Models.RequestModels;
using System.Threading.Tasks;

namespace Blog.API.Services.MailService
{
    public interface IMailService
    {
        Task SendEmail(EmailRequest req);
    }
}
