using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Parameters;
using ZhiHu.Photo.Server.DatabaseContext;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities.Bases;
using ZhiHu.Photo.Server.Extensions;
using ZhiHu.Photo.Server.Services.Interfaces.Bases;

namespace ZhiHu.Photo.Server.Services.Bases
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork _work;

        public BaseService(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<T> FindAsync(int id)
        {
            var result = await _work.GetRepository<T>().FindAsync(id);
            return result!;
        }

        public IQueryable<T> FindAllAsync(Expression<Func<T, bool>> funcWhere)
        {
            return _work.GetRepository<T>().GetAll(funcWhere);
        }

        public async Task<IPagedList<T>> QueryAsync(QueryParameter parameter)
        {
            var queryString = parameter.Search.QueryString<T>();
            return await _work.GetRepository<T>().GetPagedListAsync(queryString, pageSize: parameter.PageSize, pageIndex: parameter.PageIndex);
        }
        public async Task<IPagedList<T>> QueryAsync(Expression<Func<T, bool>> funcWhere)
        {
            return await _work.GetRepository<T>().GetPagedListAsync(funcWhere);
        }

        public async Task<T> InsertAsync(T t)
        {
            if (t == null) throw new Exception("新增数据为空");
            t.CreateDate = t.UpdateDate = DateTime.Now;
            await _work.GetRepository<T>().InsertAsync(t);
            await CommitAsync();
            return t;
        }

        public async Task<IEnumerable<T>> InsertAsync(IEnumerable<T> tList)
        {
            if (tList == null) throw new Exception("新增数据为空");

            foreach (var t in tList)
            {
                t.CreateDate = t.UpdateDate = DateTime.Now;
                await _work.GetRepository<T>().InsertAsync(t);
            }
            await CommitAsync();
            return tList;
        }

        public async Task<T> UpdateAsync(T t)
        {
            if (t == null) throw new Exception("更新数据为空");
            t.UpdateDate = DateTime.Now;
            _work.GetRepository<T>().Update(t);
            await CommitAsync();
            return t;
        }

        public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> tList)
        {
            if (tList == null) throw new Exception("更新数据为空");

            foreach (var t in tList)
            {
                t.UpdateDate = DateTime.Now;
                _work.GetRepository<T>().Update(t);
            }
            await CommitAsync();
            return tList;
        }

        public async Task DeleteAsync(int id)
        {
            var t = await FindAsync(id);
            if (t == null) throw new Exception("删除数据为空");
            await DeleteAsync(t);
        }

        public async Task DeleteAsync(T t)
        {
            if (t == null) throw new Exception("删除数据为空");
            _work.GetRepository<T>().Delete(t);
            await CommitAsync();
        }

        public async Task DeleteAsync(IEnumerable<T> tList)
        {
            if (tList == null) throw new Exception("删除数据为空");
            _work.GetRepository<T>().Delete(tList);
            await CommitAsync();
        }

        public async Task ClearTableAsync()
        {
            await Task.Yield();
            var type = typeof(T);
            var tableName = type.Name;
            var attribute = type.GetCustomAttribute<TableAttribute>();
            if (attribute != null)
            {
                tableName = attribute.Name;
            }

            _work.ExecuteSqlCommand($"truncate table {tableName}");
        }
        public async Task CommitAsync()
        {
            await _work.SaveChangesAsync();
        }

    }
}
