using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Models;

namespace ZhiHu.Photo.Services.Contacts
{
    public interface IAnswerService
    {
        ApiResponse<PagedList<AnswerDto>> GetAnswerList(int pageIndex, int pageSize);
    }
}
