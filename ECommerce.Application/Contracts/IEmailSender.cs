using ECommerce.Application.Common.ResultPattern;

namespace ECommerce.Application.Contracts
{
    public interface IEmailSender
    {
        Task<Result> SendEmailAsync(string email,string subject, string htmlMessage);
    }
}





