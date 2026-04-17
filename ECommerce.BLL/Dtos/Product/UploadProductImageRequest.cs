using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Dtos.Product
{
    public sealed record UploadProductImageRequest(IFormFile File);
   
}

