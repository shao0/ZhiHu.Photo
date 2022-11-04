using System.Linq;
using System.Reflection;
using AutoMapper;
using Newtonsoft.Json;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Common.Parameters;
using ZhiHu.Photo.Server.DatabaseContext;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Extensions;
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
            var queryString = parameter.Search.QueryString<AnswerEntity>();
            return await _work
                .GetRepository<AnswerEntity>()
                .GetAll()
                .GroupJoin(_work.GetRepository<ImageEntity>().GetAll()
                    , answer => answer
                    , i => i.Answer
                    , (a, images) => a).OrderBy(a => a.AnswerUpdatedTimeStamp)
                .Where(queryString)
                .ToPagedListAsync(parameter.PageIndex, parameter.PageSize);
        }
    }
}
