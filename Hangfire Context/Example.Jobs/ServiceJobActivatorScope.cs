using Hangfire;
using Hangfire.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Jobs
{
    internal class ServiceJobActivatorScope : JobActivatorScope
    {
        private readonly IServiceScope _serviceScope;

        public ServiceJobActivatorScope([NotNull] IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public override object Resolve(Type type)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(_serviceScope.ServiceProvider, type);
        }

        public override void DisposeScope()
        {
            _serviceScope.Dispose();
        }
    }
}
