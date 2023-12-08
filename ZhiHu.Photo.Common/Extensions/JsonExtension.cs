using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZhiHu.Photo.Common.Extensions
{
    public static class JsonExtension
    {
        /// <summary>
        /// 序列化实列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json) => JsonSerializer.Deserialize<T>(json)!;
        /// <summary>
        /// 序列化实列
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(this string json, Type type) => JsonSerializer.Deserialize(json, type)!;
        /// <summary>
        /// 实列序列化字符串json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj) => JsonSerializer.Serialize(obj);
    }
}
