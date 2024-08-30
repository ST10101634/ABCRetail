using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ABCRetail.Controllers
{
    public class FileStorageController : Controller
    {
        private readonly ShareClient _shareClient;
        private const string ShareName = "Share";

        public FileStorageController(ShareClient shareClient)
        {
            _shareClient = shareClient;
        }

        public async Task<IActionResult> Index()
        {
            var fileItems = new List<string>();

            ShareDirectoryClient directoryClient = _shareClient.GetRootDirectoryClient();
            await foreach (var fileItem in directoryClient.GetFilesAndDirectoriesAsync())
            {
                fileItems.Add(fileItem.Name);
            }

            return View(fileItems);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var directoryClient = _shareClient.GetRootDirectoryClient();
                var fileClient = directoryClient.GetFileClient(file.FileName);

                using (var stream = file.OpenReadStream())
                {
                    await fileClient.CreateAsync(stream.Length);
                    await fileClient.UploadAsync(stream);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult ConfirmDownload(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }
            return View("Download", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> Download(string fileName)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            if (await fileClient.ExistsAsync())
            {
                var downloadResponse = await fileClient.DownloadAsync();
                var memoryStream = new MemoryStream();
                await downloadResponse.Value.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                return File(memoryStream, downloadResponse.Value.ContentType, fileName);
            }

            return NotFound();
        }

        public IActionResult ConfirmDelete(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }
            return View("Delete", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            if (await fileClient.ExistsAsync())
            {
                await fileClient.DeleteAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
