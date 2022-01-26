using Example.Jobs;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(configBuilder =>
{
    configBuilder.AddJsonFile("appsettings.json", false)
               .AddJsonFile($"appsettings.development.json", true)
               .AddCommandLine(args)
               .Build();
});

builder.ConfigureServices((context, services) =>
{
    services.AddExampleJobs();

    services.AddHangfire((services, globalConfig) =>
    {
        var configuration = services.GetService(typeof(IConfiguration)) as IConfiguration;

        globalConfig.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true,
                PrepareSchemaIfNecessary = true
            });
    });

    services.AddHangfireServer((serviceProvider, options) =>
    {
        options.Activator = new CustomJobActivator(serviceProvider.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory);
    });
});

var host = builder.Build();

host.Run();