using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public sealed record UploadProductImageRequest(IFormFile File);
   
}




