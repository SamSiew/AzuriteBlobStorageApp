using Moq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AzureBlobStorageAppClient.Tests
{
    public class AzureBlobStorageMockUnitTests
    {
        private const string Content = "This file content is made by Sam today";

        [Fact]
        public async Task AzureBlobStorageClientV11Test()
        {
            // Arrange
            var mock = new Mock<IAzureBlobStorage>();

            mock
                .Setup(azureBlobStorage => azureBlobStorage.UploadTextFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() =>
                {
                    mock.Setup(azureBlobStorage => azureBlobStorage.ReadTextFile(It.IsAny<string>()))
                        .Returns(async () => await Task.FromResult(Content));
                });

            mock
                .Setup(azureBlobStorage => azureBlobStorage.DeleteTextFile(It.IsAny<string>()))
                .Callback(() =>
                {
                    mock.Setup(azureBlobStorage => azureBlobStorage.ReadTextFile(It.IsAny<string>()))
                        .Returns(async () => await Task.FromResult(string.Empty));
                });

            // Act
            await mock.Object.UploadTextFile("file.txt", Content);
            var readTextFile = await mock.Object.ReadTextFile("file.txt");

            // Assert
            Assert.Equal("This file content is made by Sam today", readTextFile);

            // Finalizing Act
            await mock.Object.DeleteTextFile("file.txt");
            var readTextFileAfterDelete = await mock.Object.ReadTextFile("file.txt");

            // Finalizing Assert
            Assert.Equal(string.Empty, readTextFileAfterDelete);
        }

        [Fact]
        public async Task AzureBlobStorageClientV12Test()
        {
            // Arrange
            var mock = new Mock<IAzureBlobStorage>();

            mock
                .Setup(azureBlobStorage => azureBlobStorage.UploadTextFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filename, data) =>
                {
                    mock.Setup(azureBlobStorage => azureBlobStorage.ReadTextFile(filename))
                        .Returns(async () => await Task.FromResult(data));
                });

            mock
                .Setup(azureBlobStorage => azureBlobStorage.DeleteTextFile(It.IsAny<string>()))
                .Callback<string>((filename) =>
                {
                    mock.Setup(azureBlobStorage => azureBlobStorage.ReadTextFile(filename))
                        .Returns(async () => await Task.FromResult(string.Empty));
                });

            // Act
            await mock.Object.UploadTextFile("file.txt", "This file content is made by Sam");
            var readTextFile = await mock.Object.ReadTextFile("file.txt");

            // Assert
            Assert.Equal("This file content is made by Sam", readTextFile);

            // Finalizing Act
            await mock.Object.DeleteTextFile("file.txt");
            var readTextFileAfterDelete = await mock.Object.ReadTextFile("file.txt");

            // Finalizing Assert
            Assert.Equal(string.Empty, readTextFileAfterDelete);
        }
    }
}
