using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using ECommerce.BLL.Abstractions.Errors.File;
using ECommerce.BLL.Abstractions.ResultPattern;
using ECommerce.BLL.Dtos.Product;

namespace ECommerce.BLL.Managers.FileManager
{
    public class FileManager : IFileManager
    {
        public async Task<Result<string>> UploadFileAsync(UploadProductImageRequest uploadFile,string baseUrl,string schema , string host)
        {
            // Implement your file upload logic here, e.g., save the file to a storage service and return the URL

            if(string.IsNullOrWhiteSpace(host)|| string.IsNullOrWhiteSpace(schema))
                return Result.Failure<string>(FileError.SchemaHostNotFound);

            var file = uploadFile.File;
            var extension = Path.GetExtension(file.FileName).ToLower();
            var fileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "_").ToLower();
            var actualFileName = $" {fileName}-{Guid.NewGuid().ToString()}{extension}";

            var directory = Path.Combine(baseUrl, "uploads"); // C:\Projects\MyApp\wwwroot\uploads

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var filePath = Path.Combine(directory, actualFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{schema}://{host}/uploads/{actualFileName}";

            return Result.Success(url);







        }
    }
}
//var directory = Path.Combine(baseUrl, "uploads");
// baseUrl ---> Physical Path of the server where you want to save the files
//var basePath = _env.WebRootPath; // C:\Projects\MyApp\wwwroot
//var directoryPath = Path.Combine(basePath, "Files"); // C:\Projects\MyApp\wwwroot\Files
//var newFileName = "myimage-1234.jpg";

//var url = $"{schema}://{host}/Files/{newFileName}";
// https://www.example.com/Files/myimage-1234.jpg

// Implement your file upload logic here, e.g., save the file to a storage service and return the URL
