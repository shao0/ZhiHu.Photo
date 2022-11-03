using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Services.Interfaces.Bases;

namespace ZhiHu.Photo.Server.Services.Interfaces
{
    public interface IScanService : IBaseService<AnswerEntity>
    {
        /// <summary>
        /// 最小更新时间插入
        /// </summary>
        /// <returns></returns>
        Task LastScanInsert();
        /// <summary>
        /// 扫描插入
        /// </summary>
        /// <returns></returns>
        Task ScanInsert();
    }
}
