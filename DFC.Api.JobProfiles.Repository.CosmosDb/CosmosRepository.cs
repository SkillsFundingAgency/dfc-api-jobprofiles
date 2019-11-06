using DFC.Api.JobProfiles.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Repository.CosmosDb
{
    public class CosmosRepository : ICosmosRepository
    {
        private readonly CosmosDbConnection cosmosDbConnection;
        private readonly IDocumentClient documentClient;

        public CosmosRepository(CosmosDbConnection cosmosDbConnection, IDocumentClient documentClient, IHostingEnvironment hostingEnvironment)
        {
            this.cosmosDbConnection = cosmosDbConnection;
            this.documentClient = documentClient;

            if (hostingEnvironment.IsDevelopment())
            {
                CreateDatabaseIfNotExistsAsync().GetAwaiter().GetResult();
                CreateCollectionIfNotExistsAsync().GetAwaiter().GetResult();
            }
        }

        private Uri DocumentCollectionUri => UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId);

        public async Task<bool> PingAsync()
        {
            var query = documentClient.CreateDocumentQuery<SummaryDataModel>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                                       .AsDocumentQuery();

            if (query == null)
            {
                return false;
            }

            var models = await query.ExecuteNextAsync<SummaryDataModel>().ConfigureAwait(false);
            var firstModel = models.FirstOrDefault();

            return firstModel != null;
        }

        public async Task<IEnumerable<SummaryDataModel>> GetSummaryListAsync()
        {
            var feedOptions = new FeedOptions
            {
                EnableCrossPartitionQuery = true,
                MaxDegreeOfParallelism = -1,
            };
            const string query = @"select c.id, c._etag, c.CanonicalName, c.BreadcrumbTitle, c.AlternativeNames "
                                 + " from c ";

            var queryable = documentClient.CreateDocumentQuery<Document>(DocumentCollectionUri, query, feedOptions).AsDocumentQuery();

            var dataModels = new List<SummaryDataModel>();
            while (queryable.HasMoreResults)
            {
                var response = await queryable.ExecuteNextAsync<SummaryDataModel>().ConfigureAwait(false);

                dataModels.AddRange(response);
            }

            return dataModels.ToArray();
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(cosmosDbConnection.DatabaseId)).ConfigureAwait(false);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await documentClient.CreateDatabaseAsync(new Database { Id = cosmosDbConnection.DatabaseId }).ConfigureAwait(false);
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
                await documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId)).ConfigureAwait(false);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    var pkDef = new PartitionKeyDefinition
                    {
                        Paths = new Collection<string>() { cosmosDbConnection.PartitionKey },
                    };

                    await documentClient.CreateDocumentCollectionAsync(
                                UriFactory.CreateDatabaseUri(cosmosDbConnection.DatabaseId),
                                new DocumentCollection { Id = cosmosDbConnection.CollectionId, PartitionKey = pkDef },
                                new RequestOptions { OfferThroughput = 1000 }).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        private Uri CreateDocumentUri(Guid documentId)
        {
            return UriFactory.CreateDocumentUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId, documentId.ToString());
        }
    }
}