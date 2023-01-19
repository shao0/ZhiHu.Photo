namespace ZhiHu.Photo.Server.Services.Interfaces.Bases
{
    public interface IScopeControl
    {
        /// <summary>
        /// 释放
        /// </summary>
        public Action ScopeDispose { get; set; }
    }
}
