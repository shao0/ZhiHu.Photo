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
using ZhiHu.Photo.Extensions;
using ZhiHu.Photo.Models;
using ZhiHu.Photo.Models.Bases;
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

        public async Task<ApiResponse<PagedList<AnswerInfo>>> GetAnswerList(int pageIndex, int pageSize)
        {
            var result = await _zhiHu.GetList(new BaseRequest { Route = $"/api/Answer/GetAll?PageIndex={pageIndex - 1}&PageSize={pageSize}" });

            return result.Map<ApiResponse<PagedList<AnswerInfo>>>();
        }
    }
}
