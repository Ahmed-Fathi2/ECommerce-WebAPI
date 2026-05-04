using ECommerce.Application.Common.Errors;
namespace ECommerce.Application.Common.ResultPattern
{
    public record Error(string Code, string Description,int? StatusCode)
    {

        public static readonly Error None= new (string.Empty,string.Empty,null);
    }
}




