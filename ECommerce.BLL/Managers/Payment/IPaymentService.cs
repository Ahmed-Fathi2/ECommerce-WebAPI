using ECommerce.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Managers.Payment
{
    public interface IPaymentService
    {
        Task<Result<string>> InitiatePaymentAsync(string origin, Guid orderId);

    }
}
