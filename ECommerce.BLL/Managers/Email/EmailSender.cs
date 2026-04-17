using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using ECommerce.Common.ResultPattern;
using ECommerce.Common.Constants;

namespace ECommerce.BLL.Managers.Email
{
    public class EmailSender(IConfiguration configuration) : IEmailSender
    {
        public string secretKey { get; set; } = configuration.GetValue<string>("SendGrid:SecretKey")!;

        public async Task<Result> SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var client = new SendGridClient(secretKey);

            var from = new EmailAddress("ahmedmhmd0237@gmail.com", "ITI-Itake-46");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            try
            {

                await client.SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                Result.Failure(new Error("Email sending failed", ex.Message, ErrorStatusCodes.BadRequest));
            }
               
                

            return Result.Success();
        }
    }
}

