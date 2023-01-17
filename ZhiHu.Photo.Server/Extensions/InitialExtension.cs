using ZhiHu.Photo.Server.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;

namespace ZhiHu.Photo.Server.Extensions
{
    public static class InitialExtension
    {
        public static void InitialStatic(this IServiceProvider serviceProvider)
        {
            IHelperService helperService = serviceProvider.GetService<IHelperService>();
            IQueryablePageListExtensions.CountDictionary = helperService.GetCountDictionary().Result;
        }
    }
}
