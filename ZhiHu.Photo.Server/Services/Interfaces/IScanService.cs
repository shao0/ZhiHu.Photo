using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Services.Interfaces.Bases;

namespace ZhiHu.Photo.Server.Services.Interfaces
{
    public interface IScanService : IScopeControl
    {
        /// <summary>
        /// 最小更新时间插入
        /// </summary>
        /// <returns></returns>
        void LastScanInsert();
        /// <summary>
        /// 扫描插入
        /// </summary>
        /// <returns></returns>
        void ScanInsert();
        /// <summary>
        /// 刷新数据Images
        /// </summary>
        /// <returns></returns>
        void LocalRefreshImage();
        /// <summary>
        /// 图片补漏
        /// </summary>
        void ImagePatching();
    }
}
