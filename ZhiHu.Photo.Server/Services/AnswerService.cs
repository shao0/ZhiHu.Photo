using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public AnswerService(IUnitOfWork work, IMapper mapper) : base(work)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<AnswerDto>> QueryAnswerAndImageAsync(QueryParameter parameter)
        {
            var pageList = await _work.GetRepository<AnswerEntity>().GetAll().OrderBy(a=> a.AnswerUpdatedTimeStamp).Include(a => a.Images)
                .ToPagedListAsync(parameter.PageIndex, parameter.PageSize);
            return _mapper.Map<PagedList<AnswerDto>>(pageList);
        }
    }
}
