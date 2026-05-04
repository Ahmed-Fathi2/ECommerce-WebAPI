using ECommerce.Domain.Repositories;
using ECommerce.Application.Common.Settings;
using ECommerce.Application.Common.Errors;
using ECommerce.Application.Common.ResultPattern;
using ECommerce.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using ECommerce.Application.Contracts;

namespace ECommerce.Application.Services
{
    public class PaymentService(IUnitOfWork unitOfWork ,
        IOptions<KashierSetting> options,
        HttpClient httpClient,
        ILogger<PaymentService> logger) : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<PaymentService> _logger = logger;
        private readonly KashierSetting _options = options.Value;

        public async Task<Result<string>> InitiatePaymentAsync(string origin, Guid orderId)
        {
           
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order is null)
                return Result.Failure<string>(OrderError.OrderNotFound);

            //  Build request body (Kashier API format)
            var requestBody = new
            {
                expireAt = DateTime.UtcNow.AddMinutes(30).ToString("o"), 
                maxFailureAttempts = 3,
                paymentType = "credit",

                amount = order.TotalAmount.ToString("F2"), 
                currency = "EGP",
               
                order = order.Id.ToString(),
                merchantId = _options.MerchantId,


                // Callback URL for redirection after payment (client-side) 
                // cilen side mean browser will be redirected to this URL after payment
                merchantRedirect = $"{origin}/payment/callback",

                //  backend webhook
                serverWebhook = $"{origin}/api/payment/webhook",

                description = $"Payment for Order {order.Id}",

                allowedMethods = "card,wallet",
                type = "one-time",
                display = "en",
                customer = new
                {
                    email = "test@test.com",
                    reference = order.ApplicationUserId
                }
            };

            // 3 Serialize to JSON
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 4 Create HTTP request 
            var request = new HttpRequestMessage(HttpMethod.Post, _options.BaseUrl);

            request.Headers.Add("Authorization", _options.Secret); // secret key
            request.Headers.Add("api-key", _options.ApiKey);       // public api key

            request.Content = content;

            // 5 Send request
            var response = await _httpClient.SendAsync(request);


            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Kashier API call failed. Status: {StatusCode}, Response: {ResponseBody}",
                    response.StatusCode, responseBody);
                return Result.Failure<string>(PaymentErrors.PaymentFailed);
            }
           
            // 8 Parse response
            using var doc = JsonDocument.Parse(responseBody);

            //  get sessionUrl for redirection
            if (!doc.RootElement.TryGetProperty("sessionUrl", out var sessionUrlElement))
            {
                _logger.LogWarning("Kashier API response missing sessionUrl. Response: {ResponseBody}", responseBody);
                return Result.Failure<string>(PaymentErrors.PaymentFailed);
            }
            

            var sessionUrl = sessionUrlElement.GetString();

            ////
            //order.SessionId = sessionUrl;
            //await _unitOfWork.SaveChangesAsync();

            // 9?? Return redirect URL
            return Result.Success(sessionUrl!);
        }
    }
    
}








