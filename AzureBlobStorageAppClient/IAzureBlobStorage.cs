using System.Threading.Tasks;

namespace AzureBlobStorageAppClient
{
    public interface IAzureBlobStorage
    {
        public Task UploadTextFile(string filename, string data);
        public Task<string> ReadTextFile(string filename);
        public Task DeleteTextFile(string filename);
    }
}
