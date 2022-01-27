using Microsoft.Extensions.DependencyInjection;

namespace Example.Jobs
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExampleJobs(this IServiceCollection services)
        {
            services.AddTransient<IExampleJob, ExampleJob>();
            services.AddScoped<IContextualDependency, ContextualDependency>();

            return services;
        }
    }
}
