using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.API.JobProfiles.Common.CosmosDbSupport
{
    public class Query
    {
        private static DocumentClient client;
        private Uri Endpoint { get; set; }
        private string AuthorisationKey { get; set; }
        private string DatabaseName { get; set; }
        private string CollectionName { get; set; }

        public Query(string databaseName, string collectionName, string accountUrl, string authorisationKey)
        {
            DatabaseName = databaseName;
            CollectionName = collectionName;
            Endpoint = new Uri(accountUrl);
            AuthorisationKey = authorisationKey;
        }

        public async Task<List<Document>> GetAllDocuments()
        {
            using (client = new DocumentClient(Endpoint, AuthorisationKey))
            {
                FeedOptions queryOptions = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = 1000 };
                IDocumentQuery<Microsoft.Azure.Documents.Document> documentQuery = client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions).AsDocumentQuery();
                FeedResponse<Document> queryResults = await documentQuery.ExecuteNextAsync<Document>();
                return queryResults.ToList();
            }
        }
    }
}
