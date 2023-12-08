using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Models.Bases;

namespace ZhiHu.Photo.DataAccess.Contacts
{
    public interface IDataAccessBase<T>
    {
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<T>> Added(BaseRequest request);
        /// <summary>
        /// 删
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse> Deleted(BaseRequest request);
        /// <summary>
        /// 改
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<T>> Updated(BaseRequest request);
        /// <summary>
        /// 查
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<PagedList<T>>> GetList(BaseRequest request);
    }
}
