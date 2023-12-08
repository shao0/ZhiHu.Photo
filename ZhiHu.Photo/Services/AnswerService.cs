using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZhiHu.Photo.Attributes;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.DataAccess.Contacts;
using ZhiHu.Photo.Services.Contacts;

namespace ZhiHu.Photo.Services
{
    [AutoInjection(ServiceLifetime.Singleton)]
    public class AnswerService : IAnswerService
    {
        private readonly IZhiHuDataAccess _zhiHu;

        public AnswerService(IZhiHuDataAccess zhiHu)
        {
            _zhiHu = zhiHu;
        }

        public ApiResponse<PagedList<AnswerDto>> GetAnswerList(int pageIndex, int pageSize)
        {
            
            return new();
        }
    }
}
