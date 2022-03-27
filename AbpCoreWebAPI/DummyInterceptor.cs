using System.Diagnostics;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace AbpCoreWebAPI
{
    public class DummyInterceptor : IAbpInterceptor, ITransientDependency
    {
        private readonly ILogger<DummyInterceptor> logger;

        public DummyInterceptor(ILogger<DummyInterceptor> logger)
        {
            this.logger = logger;
        }

        public async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            var sw = Stopwatch.StartNew();
            logger.LogInformation("Intercepting method {className}.{methodName}", invocation.Method.DeclaringType?.Name, invocation.Method.Name);
            await invocation.ProceedAsync();
            logger.LogInformation("Finished method {className}.{methodName}. Elapsed {ElapsedMs}ms", invocation.Method.DeclaringType?.Name, invocation.Method.Name, sw.ElapsedMilliseconds);
        }
    }
}