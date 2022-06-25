using Microsoft.WindowsAzure.Storage;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AzureBlobStorageAppClient.Tests
{
    public class AzureBlobStorageClientV11IntegrationTests : AzuriteContainerSetup, IAsyncLifetime
    {
        private const string Container = "testcontainer";
        private const string ConnectionString = "UseDevelopmentStorage=true";
        private IAzureBlobStorage azureBlobStorageClient;

        public Task InitializeAsync()
        {
            azureBlobStorageClient = new AzureBlobStorageClientV11(CloudStorageAccount.Parse(ConnectionString), Container);
            return Task.CompletedTask;
        }

        [Fact(DisplayName = "Should be able to Upload text file to azure blob storage ClientV11")]
        public async Task ShouldBeAbleTo_UploadTextFile_ToAzureBlobStorageClientV11()
        {
            // Arrange
            string filename = "file_1.txt";
            string content = "This file content is made by Sam today";
            await azureBlobStorageClient.UploadTextFile(filename, content);

            // Act
            var blobContainerClient = CloudStorageAccount.Parse(ConnectionString).CreateCloudBlobClient().GetContainerReference(Container);
            bool isFileExist = await blobContainerClient.GetBlockBlobReference(filename).ExistsAsync();

            // Assert
            Assert.True(isFileExist);
        }

        [Fact(DisplayName = "Should be able to Read text file from azure blob storage ClientV11")]
        public async Task ShouldBeAbleTo_ReadTextFile_FromAzureBlobStorageClientV11()
        {
            // Arrange
            string filename = "file_2.txt";
            string content = "This file content is made by Sam today";
            await azureBlobStorageClient.UploadTextFile(filename, content);

            // Act
            var readTextFile = await azureBlobStorageClient.ReadTextFile(filename);

            // Assert
            Assert.Equal("This file content is made by Sam today", readTextFile);
        }

        [Fact(DisplayName = "Should be able to Delete text file from azure blob storage ClientV11")]
        public async Task ShouldBeAbleTo_DeleteTextFile_FromAzureBlobStorageClientV11()
        {
            // Arrange
            string filename = "file_3.txt";
            string content = "This file content is made by Sam today";
            await azureBlobStorageClient.UploadTextFile(filename, content);
            await azureBlobStorageClient.DeleteTextFile(filename);

            // Act
            var blobContainerClient = CloudStorageAccount.Parse(ConnectionString).CreateCloudBlobClient().GetContainerReference(Container);
            bool isFileExist = await blobContainerClient.GetBlockBlobReference(filename).ExistsAsync();

            // Assert
            Assert.False(isFileExist);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
