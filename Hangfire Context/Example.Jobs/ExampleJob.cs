namespace Example.Jobs
{
    public class ExampleJob
    {
        private readonly IContextualDependency _contextualDependency;

        public ExampleJob(IContextualDependency contextualDependency)
        {
            _contextualDependency = contextualDependency;
        }

        public Task DoWork()
        {
            Console.WriteLine($"Doing work for tenant: {_contextualDependency.TenantId}");

            return Task.CompletedTask;
        }
    }
}