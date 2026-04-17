using ECommerce.BLL.Managers.Cart;
using ECommerce.DAL.Enums;
using ECommerce.DAL.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ECommerce.API.Controllers
{
    [ApiController]
    public class PaymentController(IUnitOfWork unitOfWork,ICartManager cartManager) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICartManager _cartManager = cartManager;

        [HttpGet("payment/callback")]
        public IActionResult Callback()
        {
            var orderId = Request.Query["merchantOrderId"];
            var status = Request.Query["paymentStatus"];

          return  status == "SUCCESS"
                  ?Ok(new { OrderId = orderId, PaymentStatus = status }) 
                  : BadRequest(new { PaymentStatus = status });
        }

        [HttpPost("api/payment/webhook")]
        public async Task<IActionResult> Webhook()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();


            var receivedSignature = Request.Headers["x-kashier-signature"].ToString();

            if (string.IsNullOrEmpty(receivedSignature))
                return Unauthorized("Missing signature");

    

            if (!ValidateSignature(body, receivedSignature, "5c390a84-b98e-404f-bf96-616e7f9d5403"))
                return Unauthorized("Invalid signature");

            var json = JsonDocument.Parse(body);
            var data = json.RootElement.GetProperty("data");

            var orderId = data.GetProperty("merchantOrderId").GetString();
            var status = data.GetProperty("status").GetString();
            var transactionId = data.GetProperty("transactionId").GetString(); 

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(Guid.Parse(orderId!));
            if (order == null)
                return NotFound();

            if (order.PaymentStatus == PaymentStatus.Paid)
                return Ok(); 

            if (status == "SUCCESS")
            {
                order.PaymentStatus = PaymentStatus.Paid;
                order.OrderStatus = OrderStatus.Confirmed;
                order.PaymentTransactionId = transactionId!;

                await  _cartManager.ClearCartAsync(order.ApplicationUserId);
            }
            else if (status == "FAILURE")
            {
                order.PaymentStatus = PaymentStatus.Failed;
                //order.OrderStatus = OrderStatus.Cancelled;
            }
        
  
            await _unitOfWork.SaveAsync();

                return Ok();
        }


        private bool ValidateSignature(string body, string receivedSignature, string secret)
        {
            var json = JsonDocument.Parse(body); 
            var data = json.RootElement.GetProperty("data");

            var keys = data.GetProperty("signatureKeys")
                           .EnumerateArray()
                           .Select(x => x.GetString())
                           .Where(x => x != null)
                           .OrderBy(x => x)
                           .ToList();


            var sb = new StringBuilder();

            foreach (var key in keys)
            {
                var value = data.GetProperty(key!).ToString();


                var encodedValue = Uri.EscapeDataString(value);

                sb.Append($"{key}={encodedValue}&");
            }

            var payload = sb.ToString().TrimEnd('&');


            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));

            var computedSignature = BitConverter.ToString(hash)
                .Replace("-", "")
                .ToLower();

            return computedSignature == receivedSignature;
        }
    }
}
