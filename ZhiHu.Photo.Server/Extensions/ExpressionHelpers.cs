using System.Linq.Expressions;
using System.Reflection;
using ZhiHu.Photo.Server.Models.Attributes;

namespace ZhiHu.Photo.Server.Extensions
{
    public static class ExpressionHelpers
    {
        /// <summary>
        /// 查询字符串
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> QueryString<TEntity>(this string query)
        {
            var properties = typeof(TEntity).GetProperties();
            var propertyList = from p in properties let attributes = p.GetCustomAttributes<QueryStringAttribute>(false) let q = attributes.FirstOrDefault<QueryStringAttribute>() where q != null select p;
            var parameter = Expression.Parameter(typeof(TEntity), "t");
            var toUpper = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);
            var isNullOrWhiteSpace = typeof(string).GetMethod("IsNullOrWhiteSpace", new[] { typeof(string) });
            var contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var search = Expression.Constant(query,typeof(string));
            var searchIsNull = Expression.Call(isNullOrWhiteSpace!, search);
            Expression last = searchIsNull;
            foreach (var p in propertyList)
            {
                var property = Expression.Property(parameter, p.Name);
                var propertyIsNullOrWhiteSpace = Expression.Call(isNullOrWhiteSpace!, property);
                var isFalse = Expression.IsFalse(propertyIsNullOrWhiteSpace);
                var propertyToUpper = Expression.Call(property, toUpper!);
                var searchToUpper = Expression.Call(search, toUpper!);
                var containsCall = Expression.Call(propertyToUpper, contains!, searchToUpper);
                var and = Expression.AndAlso(isFalse, containsCall);
                var isTrue = Expression.IsTrue(and);
                last = Expression.OrElse(last, isTrue);
            }
            return Expression.Lambda<Func<TEntity, bool>>(last, parameter);
        }
    }
}
