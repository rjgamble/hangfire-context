namespace Example.Jobs
{
    public interface IContextualDependency
    {
        int TenantId { get; set; }
    }
}