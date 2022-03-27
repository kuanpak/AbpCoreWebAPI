using Volo.Abp.DependencyInjection;

namespace AbpCoreWebAPI.AppServices
{
    public interface IDummyAppService
    {
        Task<string> HelloAsync(string name);
    }
}
