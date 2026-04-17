using ECommerce.Common.ResultPattern;
using ECommerce.BLL.Dtos.Product;

namespace ECommerce.BLL.Managers.FileManager
{
    public interface IFileManager
    {
        Task<Result<string>> UploadFileAsync(UploadProductImageRequest uploadFile, string baseUrl, string? schema, string? host);
    }
}

