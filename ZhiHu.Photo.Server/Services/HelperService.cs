using System.Reflection;
using System.Runtime.Loader;
using ZhiHu.Photo.Server.DatabaseContext;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Models.Attributes;
using ZhiHu.Photo.Server.Services.Bases;
using ZhiHu.Photo.Server.Services.Interfaces;

namespace ZhiHu.Photo.Server.Services
{
    [AutoInjection(ServiceLifetime.Singleton,true)]
    public class HelperService : IHelperService
    {
        private readonly PhotoDbContext _work;
        private Type[] Types { get; set; }

        public HelperService(PhotoDbContext work)
        {
            _work = work;
            Initial();
        }

        private void Initial()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(location);
            Types = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.Name.EndsWith("Entity")).ToArray();
        }

        public async Task<Dictionary<string, int>> GetCountDictionary()
        {
            var result = new Dictionary<string, int>();
            await Task.Yield();
            var method = _work.GetType().GetMethod("Set");
            foreach (var t in Types)
            {
                var key = t.FullName;
                var genericMethod = method.MakeGenericMethod(t);
                var obj = genericMethod.Invoke(_work, null);
                var queryableType = obj.GetType();
                var countMethod = queryableType.GetMethod("Count");
                var countGenericMethod = countMethod.MakeGenericMethod(t);
                var countObject = countGenericMethod.Invoke(obj, null);
                if (countObject is int count)
                {
                    result.Add(key, count);
                }
            }
            return result;
        }
    }
}
