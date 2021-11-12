using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle;
using DFC.Api.JobProfiles;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Data.Settings;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Api.JobProfiles.SearchServices;
using DFC.Api.JobProfiles.SearchServices.AzureSearch;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using DFC.Functions.DI.Standard;
using Dfc.SharedConfig.IoC;
using Dfc.SharedConfig.Models;
using DFC.Swagger.Standard;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
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
            var sharedConfigSettings = configuration.GetSection("SharedConfigSettings").Get<SharedConfigSettings>();
            var sharedConfigParameters = configuration.GetSection("SharedConfigParameters").Get<SharedConfigParameters>();
            var retryOptions = new RetryOptions { MaxRetryAttemptsOnThrottledRequests = 20, MaxRetryWaitTimeInSeconds = 60 };
            builder?.Services.AddSingleton<IDocumentClient>(new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey, new ConnectionPolicy { RetryOptions = retryOptions }));

            builder?.Services.AddAzureTableSharedConfigService(sharedConfigSettings).BuildServiceProvider();

            builder.AddDependencyInjection();
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            builder?.Services.AddApplicationInsightsTelemetry();
            builder?.Services.AddAutoMapper(typeof(WebJobsExtensionStartup).Assembly);
            builder?.Services.AddSingleton(cosmosDbConnection);
            builder?.Services.AddSingleton(sharedConfigParameters);
            builder?.Services.AddSingleton<IDocumentClient>(new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey));
            builder?.Services.AddSingleton<ISearchIndexClientFactory, SearchIndexClientFactory>();
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