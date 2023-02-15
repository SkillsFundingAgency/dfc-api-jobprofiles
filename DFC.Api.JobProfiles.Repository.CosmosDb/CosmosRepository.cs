using DFC.Api.JobProfiles.Data.DataModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Repository.CosmosDb
{
    [ExcludeFromCodeCoverage]
    public class CosmosRepository<T> : ICosmosRepository<T>
        where T : BaseDataModel
    {
        private readonly CosmosDbConnection cosmosDbConnection;
        private readonly CosmosClient cosmosClient;
        private readonly Container container;

        public CosmosRepository(CosmosDbConnection cosmosDbConnection, CosmosClient cosmosClient, IHostingEnvironment hostingEnvironment)
        {
            this.cosmosDbConnection = cosmosDbConnection;
            this.cosmosClient = cosmosClient;

            if (hostingEnvironment.IsDevelopment())
            {
                CreateDatabaseIfNotExistsAsync().GetAwaiter().GetResult();
                CreateCollectionIfNotExistsAsync().GetAwaiter().GetResult();
            }

            container = cosmosClient.GetContainer(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId);
        }

        public async Task<bool> PingAsync()
        {
            FeedIterator<T> resultSet = container.GetItemQueryIterator<T>(
                queryDefinition: null,
                requestOptions: new QueryRequestOptions()
                {
                    MaxItemCount = 1,
                });

            if (resultSet == null)
            {
                return false;
            }

            var models = await resultSet.ReadNextAsync().ConfigureAwait(false);
            return models.Any();
        }

        public async Task<IList<T>> GetData(Expression<Func<T, T>> selector, Expression<Func<T, bool>> filter)
        {
            List<T> dataModels = new List<T>();
            IOrderedQueryable<T> queryable = container.GetItemLinqQueryable<T>(requestOptions: new QueryRequestOptions() { MaxConcurrency = -1 });
            var matches = filter != null ? queryable.Where(filter).Select(selector) : queryable.Select(selector);

            using (FeedIterator<T> linqFeed = matches.ToFeedIterator())
            {
                while (linqFeed.HasMoreResults)
                {
                    FeedResponse<T> response = await linqFeed.ReadNextAsync();
                    foreach (var item in response)
                    {
                        dataModels.Add(item);
                    }
                }
            }

            return dataModels;
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                cosmosClient.GetDatabase(cosmosDbConnection.DatabaseId);
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbConnection.DatabaseId).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                cosmosClient.GetContainer(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId);
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    var pkDef = cosmosDbConnection.PartitionKey;
                    Database database = cosmosClient.GetDatabase(cosmosDbConnection.DatabaseId);
                    await database.CreateContainerIfNotExistsAsync(
                        id: cosmosDbConnection.CollectionId,
                        partitionKeyPath: pkDef,
                        throughput: 1000)
                        .ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}