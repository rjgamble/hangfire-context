using Example.Jobs;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace Example.Client
{
    public class BackgroundJobFilter : JobFilterAttribute, IClientFilter, IApplyStateFilter
    {
        private readonly ITenantIdHeaderProvider _tenantIdHeaderProvider;

        public BackgroundJobFilter(ITenantIdHeaderProvider tenantIdHeaderProvider)
        {
            _tenantIdHeaderProvider = tenantIdHeaderProvider;
        }

        public void OnCreated(CreatedContext filterContext)
        {
        }

        public void OnCreating(CreatingContext filterContext)
        {
            filterContext.SetJobParameter(nameof(IContextualDependency), 
                new ContextualDependency { TenantId = _tenantIdHeaderProvider.GetTenantIdFromHeader() });
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}
