using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace AbpCoreWebAPI
{
    public class DummyInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                
                context.Interceptors.TryAdd<DummyInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            return !DynamicProxyIgnoreTypes.Contains(type) && !type.IsAssignableTo<IAbpInterceptor>() && type.Name.Contains("Dummy", StringComparison.OrdinalIgnoreCase);
        }
    }
}