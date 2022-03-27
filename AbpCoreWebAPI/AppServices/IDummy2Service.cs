namespace AbpCoreWebAPI.AppServices
{
    public interface IDummy2Service
    {
        Task<string> Hello2(string name);

        string HelloSync(string name);
    }
}
