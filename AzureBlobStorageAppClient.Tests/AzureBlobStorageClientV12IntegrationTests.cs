using Azure.Storage.Blobs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AzureBlobStorageAppClient.Tests
{
    public class AzureBlobStorageClientV12IntegrationTests : AzuriteContainerSetup, IAsyncLifetime
    {
        private const string Container = "testcontainer";
        private const string ConnectionString = "UseDevelopmentStorage=true";
        private IAzureBlobStorage azureBlobStorageClient;

        public Task InitializeAsync()
        {
            azureBlobStorageClient = new AzureBlobStorageClientV12(ConnectionString, Container);
            return Task.CompletedTask;
        }

        [Fact(DisplayName = "Should be able to Upload text file to azure blob storage ClientV12")]
        public async Task ShouldBeAbleTo_UploadTextFile_ToAzureBlobStorageClientV12()
        {
            // Arrange
            string filename = "file_1.txt";
            string content = "This file content is made by Sam today";
            await azureBlobStorageClient.UploadTextFile(filename, content);

            // Act
            var blobContainerClient = new BlobContainerClient(ConnectionString, Container);
            bool isFileExist = await blobContainerClient.GetBlobClient(filename).ExistsAsync();

            // Assert
            Assert.True(isFileExist);
        }
        
        [Fact(DisplayName = "Should be able to Read text file from azure blob storage ClientV12")]
        public async Task ShouldBeAbleTo_ReadTextFile_FromAzureBlobStorageClientV12()
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

        [Fact(DisplayName = "Should be able to Delete text file from azure blob storage ClientV12")]
        public async Task ShouldBeAbleTo_DeleteTextFile_FromAzureBlobStorageClientV12()
        {
            // Arrange
            string filename = "file_3.txt";
            string content = "This file content is made by Sam today";
            await azureBlobStorageClient.UploadTextFile(filename, content);
            await azureBlobStorageClient.DeleteTextFile(filename);

            // Act
            var blobContainerClient = new BlobContainerClient(ConnectionString, Container);
            bool isFileExist = await blobContainerClient.GetBlobClient(filename).ExistsAsync();

            // Assert
            Assert.False(isFileExist);
        }
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
