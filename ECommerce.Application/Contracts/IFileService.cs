using ECommerce.Application.DTOs;
using ECommerce.Application.Common.ResultPattern;

namespace ECommerce.Application.Contracts
{
    public interface IFileService
    {
        Task<Result<string>> UploadFileAsync(UploadProductImageRequest uploadFile, string baseUrl, string? schema, string? host);
    }
}







