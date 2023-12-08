using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Models;

namespace ZhiHu.Photo.Services.Contacts
{
    public interface IAnswerService
    {
        Task<ApiResponse<PagedList<AnswerInfo>>> GetAnswerList(int pageIndex, int pageSize);
    }
}
