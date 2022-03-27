using Volo.Abp.DependencyInjection;

namespace AbpCoreWebAPI.AppServices
{
    public class Dummy2Service : IDummy2Service, ISingletonDependency
    {
        public async Task<string> Hello2(string name)
        {
            return await Task.Run(() => $"Hello 2 {name}");
        }

        public string HelloSync(string name)
        {
            return $"Hello 2 {name}";
        }
    }
}
