# Hangfire Contextual Data

> Problem: Enqueue a job on a multi-tenant Hangfire server with an injected dependency that has contextual properties

In this example, we have a custom header on an API endpoint `x-tenant-id` which we need to pass to _every_ job processed by the Hangfire server.

## Option 1: Job Signature
We could add a parameter to each job method:
```csharp

public class ExampleJob {
    public Task DoWork(int tenantId) {
        Console.WriteLine($"Doing work for tenant: {tenantId}");
    }
}

BackgroundJob.Enqueue<ExampleJob>(job => job.DoWork(tenantId));
```

This is not very elegant and introduces unnecesary pollution of the job in both setup and execution.

## Option 2: Job Parameters
Explore the repo further for the examples but here is a brief explanation.

* I'm making use of an implemented `IClientFilter` from Hangfire to set a job parameter on the client side
* I'm then creating a scoped implementation of a dependency in server side that sets the value of `TenantId` to the job parameter

## Build & Run
To run this solution yourself, 

1. Specify a value for `ConnectionStrings:HangfireConnection` in `appsettings.json` in both `Example.Client` and `Example.Server` projects
1. Start up both `Example.Server` and `Example.Client` projects
1. Using the Swagger UI "Try it yourself" feature, enter a value for `x-tenant-id` and watch the console from the `Example.Server` app print the output of the executed job
1. You can also browse to `/hangfire` to see the job execution in the Hangfire dashboard