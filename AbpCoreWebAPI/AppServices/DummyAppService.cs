using Volo.Abp.DependencyInjection;

namespace AbpCoreWebAPI.AppServices
{
    //[ExposeServices(typeof(IDummyAppService))]
    public class DummyAppService : IDummyAppService
    {
        private readonly ILogger<DummyAppService> _logger;
        private readonly IDummy2Service dummy2Service;

        public DummyAppService(ILogger<DummyAppService> logger, IDummy2Service dummy2Service)
        {
            _logger = logger;
            this.dummy2Service = dummy2Service;
        }

        public async Task<string> HelloAsync(string name)
        {
            _logger.LogInformation("Begin Hello {name}", name);
            var msg2 = await dummy2Service.Hello2(name);
            _logger.LogInformation("Hello2 async result: {msg2}", msg2);

            var msg3 = dummy2Service.HelloSync(name);
            _logger.LogInformation("Hello sync result: {msg3}", msg3);
            return $"Hello {name}!";
        }
    }
}
