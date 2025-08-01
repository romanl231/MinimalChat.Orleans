using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

await Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, siloBuilder) =>
    {
        siloBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "GreatestClusterId";
            options.ServiceId = "BestServiceIdEver";
        }).UseLocalhostClustering()
        .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000);
    })
    .RunConsoleAsync();