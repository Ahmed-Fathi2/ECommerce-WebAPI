using ECommerce.BLL.Abstractions;

namespace ECommerce.BLL.Managers.Email
{
    public interface IEmailSender
    {
        Task<Result> SendEmailAsync(string email,string subject, string htmlMessage);
    }
}

