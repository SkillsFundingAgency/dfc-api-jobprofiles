using System;
using System.Net.Http;
using Autofac.Core;
using Azure;
using Azure.Core.Serialization;
using Azure.Search.Documents;
using DFC.Api.JobProfiles.Common.Services;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.Data.DataModels;
using DFC.Api.JobProfiles.Functions;
using DFC.Api.JobProfiles.ProfileServices;
using DFC.Api.JobProfiles.Repository.CosmosDb;
using DFC.Api.JobProfiles.SearchServices;
using DFC.Api.JobProfiles.SearchServices.AzureSearch;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore;
using DFC.Common.SharedContent.Pkg.Netcore.Infrastructure;
using DFC.Common.SharedContent.Pkg.Netcore.Infrastructure.Strategy;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Middleware;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.Common.SharedContent.Pkg.Netcore.RequestHandler;
using DFC.Swagger.Standard;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

const string CosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:JobProfileSegment";
const string AzureSearchConfigAppSettings = "JobProfileSearchIndexConfig";
const string StaxGraphApiUrlAppSettings = "Cms:GraphApiUrl";
const string RedisCacheConnectionStringAppSettings = "Cms:RedisCacheConnectionString";

var configuration = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

var cosmosDbConnection = configuration.GetSection(CosmosDbConfigAppSettings).Get<CosmosDbConnection>();
var searchIndexSettings = configuration.GetSection(AzureSearchConfigAppSettings).Get<SearchIndexSettings>() ?? throw new ArgumentException("SearchIndexSettings are invalid.");
var cosmosClientOptions = new CosmosClientOptions { MaxRetryAttemptsOnRateLimitedRequests = 20, MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(60) };
var searchServiceName = searchIndexSettings.SearchServiceName;

UriBuilder uriBuilder = new()
{
    Scheme = "https",
    Host = $"{searchServiceName}.search.windows.net",
};
Uri searchServiceUri = uriBuilder.Uri;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(worker =>
    {
        worker.UseNewtonsoftJson();
        worker.Services.AddHttpClient();
        worker.Services.AddStackExchangeRedisCache(options => { options.Configuration = configuration.GetSection(RedisCacheConnectionStringAppSettings).Get<string>(); });
        worker.Services.AddSingleton<IGraphQLClient>(s =>
        {
            var option = new GraphQLHttpClientOptions()
            {
                EndPoint = new Uri(configuration.GetSection(StaxGraphApiUrlAppSettings).Get<string>() ?? throw new ArgumentNullException()),

                HttpMessageHandler = new CmsRequestHandler(s.GetService<IHttpClientFactory>(), s.GetService<IConfiguration>(), s.GetService<IHttpContextAccessor>() ?? throw new ArgumentNullException()),
            };
            var client = new GraphQLHttpClient(option, new NewtonsoftJsonSerializer());
            return client;
        });
        worker.Services.Configure<WorkerOptions>(options =>
        {
            var settings = NewtonsoftJsonObjectSerializer.CreateJsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            options.Serializer = new NewtonsoftJsonObjectSerializer(settings);
        });
        worker.UseMiddleware<FunctionContextAccessorMiddleware>();        
    })
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });
        services.ConfigureFunctionsApplicationInsights();
        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddSingleton(cosmosDbConnection);
        services.AddSingleton<SearchClient>(new SearchClient(
            endpoint: searchServiceUri,
            searchIndexSettings.SearchIndex,
            new AzureKeyCredential(searchIndexSettings.AccessKey)));
        services.AddSingleton<CosmosClient>(new CosmosClient(
            cosmosDbConnection.EndpointUrl,
            cosmosDbConnection.AccessKey,
            cosmosClientOptions));
        services.AddSingleton<IAzSearchQueryConverter, AzSearchQueryConverter>();
        services.AddSingleton<ISearchQueryService<JobProfileIndex>, DfcSearchQueryService<JobProfileIndex>>();
        services.AddSingleton<ISearchManipulator<JobProfileIndex>, JobProfileSearchManipulator>();
        services.AddSingleton<ISearchQueryBuilder, DfcSearchQueryBuilder>();
        services.AddSingleton<ILogger, Logger<Program>>();
        services.AddSingleton<IProfileDataService, ProfileDataService>();
        services.AddSingleton<ISummaryService, SummaryService>();
        services.AddSingleton<ISearchService, SearchService>();
        services.AddSingleton<ICosmosRepository<SummaryDataModel>, CosmosRepository<SummaryDataModel>>();
        services.AddSingleton<ICosmosRepository<SegmentDataModel>, CosmosRepository<SegmentDataModel>>();
        services.AddTransient<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
        services.AddScoped<ICorrelationIdProvider, RequestHeaderCorrelationIdProvider>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IResponseWithCorrelation, ResponseWithCorrelation>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<JobProfilesOverviewResponse>, JobProfileOverviewProfileSpecificQueryStrategy>();
        services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<JobProfileCurrentOpportunitiesGetbyUrlReponse>, JobProfileCurrentOpportunitiesGetByUrlStrategy>();
        services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<RelatedCareersResponse>, JobProfileRelatedCareersQueryStrategy>();
        services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<JobProfileHowToBecomeResponse>, JobProfileHowToBecomeQueryStrategy>();
        services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<JobProfileCareerPathAndProgressionResponse>, JobProfileCareerPathAndProgressionStrategy>();
        services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<JobProfileSkillsResponse>, JobProfileSkillsStrategy>();
        services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<SkillsResponse>, SkillsQueryStrategy>();
        services.AddSingleton<ISharedContentRedisInterfaceStrategyWithRedisExpiry<JobProfileApiSummaryResponse>, JobProfileApiSummaryStrategy>();

        services.AddSingleton<ISharedContentRedisInterfaceStrategyFactory, SharedContentRedisStrategyFactory>();

        services.AddScoped<ISharedContentRedisInterface, SharedContentRedis>();
        services.AddSingleton<IFunctionContextAccessor, FunctionContextAccessor>();
    })
    .Build();

host.Run();