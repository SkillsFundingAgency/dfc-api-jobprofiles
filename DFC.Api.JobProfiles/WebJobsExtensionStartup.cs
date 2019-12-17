using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle;
using DFC.Api.JobProfiles;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Api.JobProfiles.SearchServices;
using DFC.Api.JobProfiles.SearchServices.AzureSearch;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using DFC.Functions.DI.Standard;
using DFC.Swagger.Standard;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Search;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: WebJobsStartup(typeof(WebJobsExtensionStartup), "Web Jobs Extension Startup")]

namespace DFC.Api.JobProfiles
{
    [ExcludeFromCodeCoverage]
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        public const string CosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:JobProfileSegment";

        public void Configure(IWebJobsBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var cosmosDbConnection = configuration.GetSection(CosmosDbConfigAppSettings).Get<CosmosDbConnection>();
            var documentClient = new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey);
            var jobProfileSearchIndexConfig = configuration.GetSection(nameof(JobProfileSearchIndexConfig)).Get<JobProfileSearchIndexConfig>();
            var searchIndexClient = new SearchIndexClient(jobProfileSearchIndexConfig.SearchServiceName, jobProfileSearchIndexConfig.SearchIndex, new SearchCredentials(jobProfileSearchIndexConfig.AccessKey));

            builder.AddDependencyInjection();
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            builder?.Services.AddApplicationInsightsTelemetry();
            builder?.Services.AddAutoMapper(typeof(WebJobsExtensionStartup).Assembly);
            builder?.Services.AddSingleton(cosmosDbConnection);
            builder?.Services.AddSingleton<IDocumentClient>(documentClient);
            builder?.Services.AddSingleton(jobProfileSearchIndexConfig);
            builder?.Services.AddSingleton<ISearchIndexClient>(searchIndexClient);
            builder?.Services.AddSingleton<IAzSearchQueryConverter, AzSearchQueryConverter>();
            builder?.Services.AddSingleton<ISearchQueryService<JobProfileIndex>, DfcSearchQueryService<JobProfileIndex>>();
            builder?.Services.AddSingleton<ISearchManipulator<JobProfileIndex>, JobProfileSearchManipulator>();
            builder?.Services.AddSingleton<ISearchQueryBuilder, DfcSearchQueryBuilder>();
            builder?.Services.AddSingleton<ILogger, Logger<WebJobsExtensionStartup>>();
            builder?.Services.AddSingleton<IProfileDataService, ProfileDataService>();
            builder?.Services.AddSingleton<ISummaryService, SummaryService>();
            builder?.Services.AddSingleton<ISearchService, SearchService>();
            builder?.Services.AddSingleton<ICosmosRepository<SummaryDataModel>, CosmosRepository<SummaryDataModel>>();
            builder?.Services.AddSingleton<ICosmosRepository<SegmentDataModel>, CosmosRepository<SegmentDataModel>>();
            builder?.Services.AddTransient<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
            builder?.Services.AddScoped<ICorrelationIdProvider, RequestHeaderCorrelationIdProvider>();
            builder?.Services.AddScoped<ILogService, LogService>();
            builder?.Services.AddScoped<IResponseWithCorrelation, ResponseWithCorrelation>();
        }
    }
}