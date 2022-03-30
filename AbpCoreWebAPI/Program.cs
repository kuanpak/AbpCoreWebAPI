using AbpCoreWebAPI;
using AbpCoreWebAPI.Configuration;
using Prometheus;
using Prometheus.DotNetRuntime;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureAppConfiguration((host, configBuilder) =>
    {
        //in .NET 6, configBuilder is the same ConfigurationManager instance, the Build() just return itself.
        //create a new configuration builder to add the original sources in order not to mix up with ConfigurationManager
        var _builder = new ConfigurationBuilder();
        foreach(var source in configBuilder.Sources)
        {
            _builder.Add(source);
        }
        var _config = _builder.Build();
        
        //clear the original configuration sources and add the previous built configuration as the chained configuration source
        configBuilder.Sources.Clear();

        configBuilder.Add(new DecryptedConfigurationSource() { Configuration = _config});
    })
    .UseSerilog((hostBuilder, sp, loggerConfig) =>
    {
        loggerConfig
            .MinimumLevel.Information()
            .WriteTo.Async(c => c.Console())
            .WriteTo.Debug()
            .Enrich.FromLogContext()
            ;
    })
    .UseAutofac();

await builder.AddApplicationAsync<HttpApiHostModule>();

var collector = DotNetRuntimeStatsBuilder.Customize()
                .WithContentionStats(CaptureLevel.Informational)
                .WithGcStats(CaptureLevel.Informational)
                .WithThreadPoolStats(CaptureLevel.Informational)
                .WithExceptionStats(CaptureLevel.Errors)
                .WithJitStats(CaptureLevel.Informational)
                .WithSocketStats()
                .StartCollecting();

//var registration = EventCounterAdapter.StartListening();
//var meters = MeterAdapter.StartListening();

var app = builder.Build();
await app.InitializeApplicationAsync();

await app.RunAsync();