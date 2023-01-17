namespace ZhiHu.Photo.Server.Services.Interfaces
{
    public interface IHelperService
    {
        public Task<Dictionary<string, int>> GetCountDictionary();
    }
}
