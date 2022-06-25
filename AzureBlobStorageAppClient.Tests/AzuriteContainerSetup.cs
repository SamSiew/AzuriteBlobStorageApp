using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AzureBlobStorageAppClient.Tests
{
    public abstract class AzuriteContainerSetup
    {
        private const string ContainerImageUri = "mcr.microsoft.com/azure-storage/azurite";
        private static readonly DockerClient dockerClient = new DockerClientConfiguration().CreateClient();
        private string ContainerId { get; set; }
        private string ContainerName { get; set; }

        public AzuriteContainerSetup()
        {
            PullImage();
            StartContainer();
        }

        private void PullImage()
        {
            dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = ContainerImageUri,
                Tag = "latest"
            },
            new AuthConfig(),
            new Progress<JSONMessage>());
        }

        private void StartContainer()
        {
            ContainerName = $"azurite-{Guid.NewGuid()}";
            var response = dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = ContainerImageUri,
                Name = ContainerName,
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    {
                        "10000", default(EmptyStruct)
                    }
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {"10000", new List<PortBinding> {new PortBinding {HostPort = "10000"}}}
                    },
                    PublishAllPorts = true
                }
            });
            ContainerId = response.Result.ID;
            dockerClient.Containers.StartContainerAsync(ContainerId, null);
        }
    }
}
