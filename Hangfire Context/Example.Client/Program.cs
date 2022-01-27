using Example.Client;
using Example.Jobs;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<TenantHeaderOperationFilter>();
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ITenantIdHeaderProvider, TenantIdHeaderProvider>();
builder.Services.AddExampleJobs();

builder.Services.AddHangfire((services, options) =>
{
    var config = services.GetRequiredService<IConfiguration>();

    options.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(config.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true,
            PrepareSchemaIfNecessary = false
        });
});

var app = builder.Build();

GlobalConfiguration.Configuration.UseFilter(new BackgroundJobFilter(app.Services.GetService<ITenantIdHeaderProvider>()));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();

app.MapGet("/enqueue", (HttpContext context) =>
{
    var jobId = BackgroundJob.Enqueue<IExampleJob>(job => job.DoWork());

    return new
    {
        JobId = jobId,
    };
})
.WithName("Enqueue");

app.Run();