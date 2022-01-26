using Hangfire;
using Hangfire.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Jobs
{
    public class CustomJobActivator : JobActivator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CustomJobActivator([NotNull] IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override object ActivateJob(Type jobType)
        {
            return base.ActivateJob(jobType);
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            var jobParam = context.GetJobParameter<ContextualDependency>(nameof(IContextualDependency));

            var serviceScope = _serviceScopeFactory.CreateScope();
             
            var currentContextualDependency = serviceScope.ServiceProvider.GetRequiredService<IContextualDependency>();
            currentContextualDependency.TenantId = jobParam.TenantId;

            return new ServiceJobActivatorScope(serviceScope);
        }
    }
}
