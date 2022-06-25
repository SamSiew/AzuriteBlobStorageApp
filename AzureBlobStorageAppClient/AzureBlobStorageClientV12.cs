using Azure.Storage.Blobs;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace AzureBlobStorageAppClient
{
    public class AzureBlobStorageClientV12 : IAzureBlobStorage
    {
        private readonly BlobContainerClient blobContainerClient;

        public AzureBlobStorageClientV12(string connectionString, string container)
        {
            blobContainerClient = new BlobContainerClient(connectionString, container);
            blobContainerClient.CreateIfNotExists();
        }

        public async Task UploadTextFile(string filename, string data)
        {
            var blob = blobContainerClient.GetBlobClient(filename);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data), false))
            {
                await blob.UploadAsync(ms, CancellationToken.None);
            }
        }

        public async Task<string> ReadTextFile(string filename)
        {
            var blob = blobContainerClient.GetBlobClient(filename);
            if (await blob.ExistsAsync() == false) return string.Empty;
            var downloadedContent = await blob.DownloadStreamingAsync();
            StreamReader reader = new StreamReader(downloadedContent.Value.Content);
            return await reader.ReadToEndAsync();
        }

        public async Task DeleteTextFile(string filename)
        {
            var blob = blobContainerClient.GetBlobClient(filename);
            await blob.DeleteAsync();
        }
    }
}
