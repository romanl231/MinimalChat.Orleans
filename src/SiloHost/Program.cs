using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using System.Net;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); 
    })
    .UseOrleans((ctx, siloBuilder) =>
    {
        var connectStr = ctx.Configuration.GetConnectionString("SqlStorage");

        siloBuilder.Configure<EndpointOptions>(options =>
        {
            options.AdvertisedIPAddress = IPAddress.Loopback;
            options.SiloPort = 11111;
            options.GatewayPort = 30000;
            options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Loopback, 11111);
            options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Loopback, 30000);
        });

        siloBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = ctx.Configuration["Orleans:ClusterId"];
            options.ServiceId = ctx.Configuration["Orleans:ServiceId"];
        })
        .UseLocalhostClustering()
        .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
        .AddAdoNetGrainStorageAsDefault(options =>
        {
            options.Invariant = "System.Data.SqlClient";
            options.ConnectionString = connectStr;
        });

        siloBuilder.ConfigureLogging(logging => logging.AddConsole());
    })
    .RunConsoleAsync();