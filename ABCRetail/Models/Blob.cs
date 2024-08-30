namespace ABCRetail.Models
{
    public class ImageUploadModel
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
        public string BlobUri { get; set; }
    }

    public class ImageEditModel
    {
        public string BlobName { get; set; }
        public string NewFileName { get; set; }
        public IFormFile File { get; set; }
        public string BlobUri { get; set; }
    }

    public class ImageDeleteModel
    {
        public string BlobName { get; set; }
    }
}
