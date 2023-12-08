using System;
using System.Collections.Generic;

namespace ZhiHu.Photo.Configurations.Models
{
    public class DataAccessConfig : Dictionary<string, string>
    {
        public string Get(string key)
        {
            if (!ContainsKey(key)) throw new Exception("未找到数据层ZhiHu配置");
            return this[key];
        }
    }
}
