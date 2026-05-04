using ECommerce.Application.Validators;
using ECommerce.Application.DTOs;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class UploadProductImageRequestValidator:AbstractValidator<UploadProductImageRequest>
    {

        public readonly string[] BlockedSignature = ["4D-5A", "2F-2A", "D0-CF"];  // exe, msi, js
        public readonly string[] AllowedImagesExtensions = [".jpg", ".jpeg", ".png"];  
        public UploadProductImageRequestValidator()
        {



            // File_Size
            RuleFor(x => x.File)
                .Must((request, context) => request.File.Length < 2 * 1024 * 1024)
                .WithMessage("File size must be less than 2MB.")
                .When(x => x.File is not  null);


            //File_Content_Signature
            RuleFor(x => x.File)
            .Must((request, context) =>
            {
                BinaryReader binary = new (request.File.OpenReadStream());
                var bytes = binary.ReadBytes(2); // 2B of Signature
                var signature = BitConverter.ToString(bytes); // convert to Hexadecimal String


                foreach(var sign in BlockedSignature)
                { 
                    if(signature.Equals(sign))
                        return false;
                }

                return true;
            })
            .WithMessage("File type is not allowed.")
            .When(x=>x.File is not null);




            //File_Extension

            RuleFor(x => x.File)
            .Must((request, context) =>
            {
               var extension = Path.GetExtension(request.File.FileName).ToLower();
                foreach(var allowedExtension in AllowedImagesExtensions)
                { 
                    if(extension.Equals(allowedExtension))
                        return true;
                }
                return false;

            })
            .WithMessage("File extension is not allowed.")
            .When(x=>x.File is not null);

            //FileName
            RuleFor(x => x.File.FileName)
              .Matches("^[A-Za-z0-9_\\-.]*$")
              .WithMessage("Invalid file name. Only letters (A-Z, a-z), numbers (0-9), underscore (_), dash (-), and dot (.) are allowed.")
              .When(x => x.File is not null);




        }
    }
}




