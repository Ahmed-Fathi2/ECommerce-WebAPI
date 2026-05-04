using ECommerce.Application.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Contracts
{
    public interface IPaymentService
    {
        Task<Result<string>> InitiatePaymentAsync(string origin, Guid orderId);

    }
}




