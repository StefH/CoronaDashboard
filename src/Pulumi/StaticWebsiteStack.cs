﻿using System.IO;
using Pulumi.Azure.Constants;
using Pulumi.Azure.Core;
using Pulumi.Azure.Extensions.Storage;
using Pulumi.Azure.Storage;
using Pulumi.Azure.Storage.Inputs;

namespace Pulumi.Azure.StaticWebsite
{
    class StaticWebsiteStack : Stack
    {
        public StaticWebsiteStack()
        {
            // Use existing an Azure Resource Group
            ResourceGroup resourceGroup = ResourceGroup.Get("stef-rg-static-websites", "/subscriptions/ae9255af-d099-4cdc-90a7-241ccb29df68/resourceGroups/stef-rg-static-websites");

            // Create an Azure Storage Account
            var storageAccount = new Account("coronadashboard", new AccountArgs
            {
                ResourceGroupName = resourceGroup.Name,
                EnableHttpsTrafficOnly = true,
                AccountReplicationType = StorageAccountReplicationTypes.LRS,
                AccountTier = StorageAccountTiers.Standard,
                AccountKind = StorageAccountKinds.StorageV2,
                StaticWebsite = new AccountStaticWebsiteArgs
                {
                    IndexDocument = "index.html",
                    Error404Document = "404.html"
                }
            });

            // Upload the files from local to azure storage account
            string wwwFolder = Path.Combine("docs-temp", "wwwroot");
            string currentDirectory = Directory.GetCurrentDirectory();
            var rootDirectory = Directory.GetParent(Directory.GetParent(currentDirectory).FullName);
            string sourceFolder = Path.Combine(rootDirectory.FullName, wwwFolder);

            var blobCollectionArgs = new BlobCollectionArgs
            {
                // Required
                Source = sourceFolder,
                Type = BlobTypes.Block,
                StorageAccountName = storageAccount.Name,
                StorageContainerName = "$web",

                // Optional
                AccessTier = BlobAccessTiers.Hot
            };

            var blobCollectionOptions = new ComponentResourceOptions
            {
                Parent = storageAccount
            };
            var blobCollection = new BlobCollection("coronadashboard-static-website", blobCollectionArgs, blobCollectionOptions);

            // Export the Web address string for the storage account
            StaticEndpoint = storageAccount.PrimaryWebEndpoint;
        }

        [Output]
        public Output<string> StaticEndpoint { get; set; }
    }
}