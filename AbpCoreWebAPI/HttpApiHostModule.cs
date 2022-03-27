using AbpCoreWebAPI.AppServices;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpCoreWebAPI
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAutofacModule)
        )]
    public class HttpApiHostModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(DummyInterceptorRegistrar.RegisterIfNeeded);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            // Add services to the container.

            context.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            context.Services.AddEndpointsApiExplorer();
            context.Services.AddSwaggerGen();

            context.Services.AddSingleton<IDummyAppService, DummyAppService>();
            context.Services.AddSingleton<IDummy2Service, Dummy2Service>();

            context.Services.AddOpenTelemetryTracing(builder =>
            {
                var asmName = Assembly.GetExecutingAssembly().GetName();
                builder
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(asmName.Name, serviceVersion: asmName.Version?.ToString()))
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:4317");
                    })
                    ;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            // Configure the HTTP request pipeline.
            app.UseCorrelationId();
            app.UseRouting();

            app.UseHttpMetrics();
            app.UseMetricServer();

            //if (env.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseAuthorization();
            app.UseAbpSerilogEnrichers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapMetrics();
            });

        }
    }
}
