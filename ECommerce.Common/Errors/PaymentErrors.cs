using ECommerce.Common.Constants;
using ECommerce.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Common.Errors
{
    public class PaymentErrors
    {

        public static readonly Error PaymentFailed = new Error
        (
            Code: "PaymentFailed",
            Description: "Payment processing failed. Please try again.",
            StatusCode: ErrorStatusCodes.BadRequest
        );
        
        // public static readonly Error InvalidPaymentDetails = new Error
        //{
        //    Code = "InvalidPaymentDetails",
        //    Message = "The provided payment details are invalid. Please check and try again."
        //};
        // public static readonly Error InsufficientFunds = new Error
        //{
        //    Code = "InsufficientFunds",
        //    Message = "Insufficient funds in the account. Please use a different payment method or add funds."
        //};
        // public static readonly Error PaymentGatewayError = new Error
        //{
        //    Code = "PaymentGatewayError",
        //    Message = "An error occurred with the payment gateway. Please try again later."
        //};
    }
}
