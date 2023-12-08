using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZhiHu.Photo.Attributes;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Configurations.Models;
using ZhiHu.Photo.DataAccess.Contacts;

namespace ZhiHu.Photo.DataAccess
{
    [AutoInjection(ServiceLifetime.Singleton)]
    public class ZhiHuDataAccess : DataAccessBase<AnswerDto>, IZhiHuDataAccess
    {
        private const string KEY = "ZhiHu";
        public ZhiHuDataAccess(DataAccessConfig config) : base(config.Get(KEY))
        {
        }
    }
}
