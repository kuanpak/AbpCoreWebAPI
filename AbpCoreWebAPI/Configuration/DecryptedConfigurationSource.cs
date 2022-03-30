namespace AbpCoreWebAPI.Configuration
{
    public class DecryptedConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// The chained configuration.
        /// </summary>
        public IConfiguration Configuration { get; set; } = new ConfigurationManager();

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DecryptedConfigurationProvider(this);
        }
    }
}
