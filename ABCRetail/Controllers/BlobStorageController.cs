using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Service;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ABCRetail.Controllers
{
    public class BlobStorageController : Controller
    {
        private readonly BlobStorageService _blobService;

        public BlobStorageController(BlobStorageService blobService)
        {
            _blobService = blobService;
        }

        // GET: /BlobStorage/Index
        public async Task<IActionResult> Index()
        {
            var blobs = await _blobService.ListImagesAsync("images");
            return View(blobs);
        }

        // GET: /BlobStorage/UploadImage
        public IActionResult UploadImage()
        {
            return View();
        }

        // POST: /BlobStorage/UploadImage
        [HttpPost]
        public async Task<IActionResult> UploadImage(ImageUploadModel model)
        {
            if (model.File != null && model.File.Length > 0)
            {
                using (var stream = model.File.OpenReadStream())
                {
                    model.BlobUri = await _blobService.UploadImageAsync("images", model.FileName, stream);
                }
            }
            return View("UploadSuccess", model);
        }

        // GET: /BlobStorage/EditImage/{blobName}
        public async Task<IActionResult> EditImage(string blobName)
        {
            var blobUri = await _blobService.GetBlobUriAsync("images", blobName);
            if (blobUri == null)
            {
                return NotFound();
            }

            var model = new ImageEditModel
            {
                BlobName = blobName,
                BlobUri = blobUri
            };

            return View(model);
        }

        // POST: /BlobStorage/EditImage
        [HttpPost]
        public async Task<IActionResult> EditImage(ImageEditModel model)
        {
            if (model.File != null && model.File.Length > 0)
            {
                using (var stream = model.File.OpenReadStream())
                {
                    await _blobService.DeleteImageAsync("images", model.BlobName); // Delete old image
                    model.BlobUri = await _blobService.UploadImageAsync("images", model.NewFileName, stream); // Upload new image
                }
            }
            return View("UploadSuccess", model);
        }

        // GET: /BlobStorage/DeleteImage/{blobName}
        public IActionResult DeleteImage(string blobName)
        {
            var model = new ImageDeleteModel
            {
                BlobName = blobName
            };
            return View(model);
        }

        // POST: /BlobStorage/DeleteImage
        [HttpPost, ActionName("DeleteImage")]
        public async Task<IActionResult> DeleteImageConfirmed(string blobName)
        {
            await _blobService.DeleteImageAsync("images", blobName);
            return RedirectToAction("Index");
        }
    }
}
