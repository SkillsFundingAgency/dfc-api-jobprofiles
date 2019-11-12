using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle;
using DFC.Api.JobProfiles;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Functions.DI.Standard;
using DFC.Swagger.Standard;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

[assembly: WebJobsStartup(typeof(WebJobsExtensionStartup), "Web Jobs Extension Startup")]

namespace DFC.Api.JobProfiles
{
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

            builder.AddDependencyInjection();
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            builder?.Services.AddApplicationInsightsTelemetry();
            builder?.Services.AddAutoMapper(typeof(WebJobsExtensionStartup).Assembly);
            builder?.Services.AddSingleton(cosmosDbConnection);
            builder?.Services.AddSingleton<IDocumentClient>(documentClient);
            builder?.Services.AddSingleton<ILogger, Logger<WebJobsExtensionStartup>>();
            builder?.Services.AddSingleton<IProfileDataService, ProfileDataService>();
            builder?.Services.AddSingleton<ISummaryService, SummaryService>();
            builder?.Services.AddSingleton<ICosmosRepository<SummaryDataModel>, CosmosRepository<SummaryDataModel>>();
            builder?.Services.AddSingleton<ICosmosRepository<SegmentDataModel>, CosmosRepository<SegmentDataModel>>();
            builder?.Services.AddTransient<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
        }
    }
}