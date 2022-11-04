using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Parameters;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Services.Interfaces.Bases;

namespace ZhiHu.Photo.Server.Services.Interfaces
{
    public interface IAnswerService:IBaseService<AnswerEntity>
    {

        /// <summary>
        /// 根据查询参数查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<IPagedList<AnswerDto>> QueryAnswerAndImageAsync(QueryParameter parameter);
    }
}
