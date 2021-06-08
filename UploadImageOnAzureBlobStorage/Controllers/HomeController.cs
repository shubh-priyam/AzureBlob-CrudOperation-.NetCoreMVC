using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.Threading.Tasks;
using UploadImageOnAzureBlobStorage.Models;

namespace UploadImageOnAzureBlobStorage.Controllers
{
    public class HomeController : Controller
    {
        public readonly IConfiguration Configuration;
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
      
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ImageModel imageModel)
        {
            if (ModelState.IsValid)
            {
                var cloudBlockBlob = UploadFileToBlob(imageModel.file.FileName);
                cloudBlockBlob.Properties.ContentType = imageModel.file.ContentType;
                await cloudBlockBlob.UploadFromStreamAsync(imageModel.file.OpenReadStream());
                var imageUrl = cloudBlockBlob.Uri.AbsoluteUri.ToString();
                ViewBag.imagename = imageModel.file.FileName;
                ViewBag.imageurl = imageUrl;             
            }
            return View();
        }

        public async Task<IActionResult> Delete(string name) 
        {
            //Uri uriObj = new Uri(name);
            //string BlobName = Path.GetFileName(uriObj.LocalPath);
            var cloudBlockBlob = UploadFileToBlob(name);
            await cloudBlockBlob.DeleteAsync();
            ViewBag.status = "deleted";
            return View("Index");
        }

        private CloudBlockBlob UploadFileToBlob(string fileName)
        {
            string accessKey = Configuration.GetValue<string>("AzureStorage:AccessKey");
            string containerName = Configuration.GetValue<string>("AzureStorage:ContainerName");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            return cloudBlockBlob;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
