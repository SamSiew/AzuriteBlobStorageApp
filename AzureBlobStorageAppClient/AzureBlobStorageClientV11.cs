using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzureBlobStorageAppClient
{
    public class AzureBlobStorageClientV11 : IAzureBlobStorage
    {
        private CloudBlobContainer CloudBlobContainer { get; set; }
        public AzureBlobStorageClientV11(CloudStorageAccount cloudStorageAccount, string containername)
        {
            this.CloudBlobContainer = cloudStorageAccount.CreateCloudBlobClient().GetContainerReference(containername);
            this.CloudBlobContainer.CreateIfNotExistsAsync();
        }

        public async Task UploadTextFile(string filename, string data)
        {
            var blobReference = CloudBlobContainer.GetBlockBlobReference(filename);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data), false))
            {
                await blobReference.UploadFromStreamAsync(ms);
            }
        }

        public async Task<string> ReadTextFile(string filename)
        {
            var blobReference = CloudBlobContainer.GetBlockBlobReference(filename);
            var blobContentAsString = await blobReference.DownloadTextAsync();
            return blobContentAsString;
        }

        public async Task DeleteTextFile(string filename)
        {
            var blobReference = CloudBlobContainer.GetBlockBlobReference(filename);
            await blobReference.DeleteAsync();
        }
    }
}
