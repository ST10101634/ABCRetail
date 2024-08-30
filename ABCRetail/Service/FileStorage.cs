using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ABCRetail.Service
{
    public class FileStorageService
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorageService"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for Azure Storage Account.</param>
        public FileStorageService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Uploads a file to Azure File Storage.
        /// </summary>
        /// <param name="shareName">The name of the file share.</param>
        /// <param name="fileName">The name of the file to upload.</param>
        /// <param name="fileStream">The stream of the file to upload.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UploadFileAsync(string shareName, string fileName, Stream fileStream)
        {
            if (string.IsNullOrEmpty(shareName))
                throw new ArgumentNullException(nameof(shareName));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));

            try
            {
                ShareClient shareClient = new ShareClient(_connectionString, shareName);
                await shareClient.CreateIfNotExistsAsync();

                ShareDirectoryClient directoryClient = shareClient.GetRootDirectoryClient();
                ShareFileClient fileClient = directoryClient.GetFileClient(fileName);

                await fileClient.CreateAsync(fileStream.Length);
                await fileClient.UploadAsync(fileStream);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to upload file to Azure File Storage.", ex);
            }
        }

        /// <summary>
        /// Downloads a file from Azure File Storage.
        /// </summary>
        /// <param name="shareName">The name of the file share.</param>
        /// <param name="fileName">The name of the file to download.</param>
        /// <param name="destinationStream">The stream where the downloaded file content will be copied to.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DownloadFileAsync(string shareName, string fileName, Stream destinationStream)
        {
            if (string.IsNullOrEmpty(shareName))
                throw new ArgumentNullException(nameof(shareName));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (destinationStream == null)
                throw new ArgumentNullException(nameof(destinationStream));

            try
            {
                ShareClient shareClient = new ShareClient(_connectionString, shareName);
                ShareDirectoryClient directoryClient = shareClient.GetRootDirectoryClient();
                ShareFileClient fileClient = directoryClient.GetFileClient(fileName);

                ShareFileDownloadInfo download = await fileClient.DownloadAsync();
                await download.Content.CopyToAsync(destinationStream);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to download file from Azure File Storage.", ex);
            }
        }

       
        public async Task DeleteFileAsync(string shareName, string fileName)
        {
            if (string.IsNullOrEmpty(shareName))
                throw new ArgumentNullException(nameof(shareName));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            try
            {
                ShareClient shareClient = new ShareClient(_connectionString, shareName);
                ShareDirectoryClient directoryClient = shareClient.GetRootDirectoryClient();
                ShareFileClient fileClient = directoryClient.GetFileClient(fileName);

                await fileClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete file from Azure File Storage.", ex);
            }
        }
    }
}
