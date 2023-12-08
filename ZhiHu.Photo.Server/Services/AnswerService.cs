using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Common.Parameters;
using ZhiHu.Photo.Server.DatabaseContext;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Extensions;
using ZhiHu.Photo.Server.Models.Attributes;
using ZhiHu.Photo.Server.Models.ZhiHu;
using ZhiHu.Photo.Server.Services.Bases;
using ZhiHu.Photo.Server.Services.Interfaces;

namespace ZhiHu.Photo.Server.Services
{
    public class AnswerService : BaseService<AnswerEntity>, IAnswerService
    {

        public AnswerService(IUnitOfWork work) : base(work)
        {
        }

        public async Task<IPagedList<AnswerEntity>> QueryAnswerAndImageAsync(QueryParameter parameter)
        {
            return await _work.GetRepository<AnswerEntity>()
                .GetAll().Include(a => a.Images).ThenInclude(a=> a.Video)
                .ToPagedListAsync(parameter.PageIndex, parameter.PageSize);
        }
    }
}
