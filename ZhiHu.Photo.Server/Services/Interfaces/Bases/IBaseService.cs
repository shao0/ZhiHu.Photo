using System.Linq.Expressions;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Common.Parameters;

namespace ZhiHu.Photo.Server.Services.Interfaces.Bases
{
    public interface IBaseService<T> where T : class
    {
        #region 查询
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindAsync(int id);
        /// <summary>
        /// 根据表达式条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        IQueryable<T> FindAllAsync(Expression<Func<T, bool>> funcWhere);
        /// <summary>
        /// 根据查询参数查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<IPagedList<T>> QueryAsync(QueryParameter parameter);
        /// <summary>
        /// 根据表达式条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        Task<IPagedList<T>> QueryAsync(Expression<Func<T, bool>> funcWhere);
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<T> InsertAsync(T t);
        /// <summary>
        /// 多条新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> InsertAsync(IEnumerable<T> tList);
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        Task<T> UpdateAsync(T t);
        /// <summary>
        /// 多条更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> tList);
        #endregion

        #region 删除
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        Task DeleteAsync(int id);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        Task DeleteAsync(T t);
        /// <summary>
        /// 多条删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        Task DeleteAsync(IEnumerable<T> tList);
        #endregion

        /// <summary>
        /// 提交编辑
        /// </summary>
        Task CommitAsync();
    }
}
