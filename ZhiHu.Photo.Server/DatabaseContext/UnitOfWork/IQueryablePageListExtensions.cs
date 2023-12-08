using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.EntityFrameworkCore;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Models;

namespace ZhiHu.Photo.Server.DatabaseContext.UnitOfWork
{
    public static class IQueryablePageListExtensions
    {
        //public static Dictionary<string, int> CountDictionary { get; set; } = new();
       

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="pageSize"/> and <paramref name="pageSize"/>.
        /// 根据起始页和页大小转换成源
        /// </summary>
        /// <typeparam name="T">The type of the source.源的类型</typeparam>
        /// <param name="pageIndex">The source to paging.分页的源</param>
        /// <param name="pageSize">The index of the page.起始页</param>
        /// <param name="cancellationToken">The size of the page.页大小</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        ///     在等待任务完成时观察
        /// </param>
        /// <param name="indexFrom">The start index value.值的起始索引</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.接口继承的实例</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            //如果索引比起始页大，则异常
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            int count;

            //var key = typeof(T).FullName;
            //if (CountDictionary.ContainsKey(key))
            //{
            //    count = CountDictionary[key];
            //}
            //else
            //{
            //    //数据源大小
            //    count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            //    CountDictionary.Add(key, count);
            //}
            count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

            var items = await source.Skip((pageIndex - indexFrom) * pageSize)
                                    .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int) Math.Ceiling(count / (double) pageSize)
            };

            return pagedList;
        }
    }
}
