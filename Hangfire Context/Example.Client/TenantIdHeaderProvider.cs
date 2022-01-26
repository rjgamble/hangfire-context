namespace Example.Client
{
    public class TenantIdHeaderProvider : ITenantIdHeaderProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TenantIdHeaderProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int GetTenantIdFromHeader()
        {
            var tenantIdHeaderValue = _contextAccessor.HttpContext.Request.Headers["x-tenant-id"].Single();
            return int.Parse(tenantIdHeaderValue);
        }
    }
}
