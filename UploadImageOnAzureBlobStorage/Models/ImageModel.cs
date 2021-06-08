using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UploadImageOnAzureBlobStorage.Models
{
    public class ImageModel
    {
        [Required(ErrorMessage ="Please Upload a file")]
        public IFormFile file { get; set; }
    }
}
