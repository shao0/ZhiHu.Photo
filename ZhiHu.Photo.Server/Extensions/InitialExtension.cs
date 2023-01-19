namespace ZhiHu.Photo.Server.Extensions
{
    public static class InitialExtension
    {
        public static void Initial(this IServiceProvider provider)
        {
            ScopeExtension.Provider = provider;
        }
    }
}
