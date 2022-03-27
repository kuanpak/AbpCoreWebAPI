using AbpCoreWebAPI;
using Prometheus;
using Prometheus.DotNetRuntime;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host
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