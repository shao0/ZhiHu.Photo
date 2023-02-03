namespace ZhiHu.Photo.Server.Helpers
{
    public static class WebHelper
    {
        private static string[] UserAgent { get; set; } = {
            "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:30.0) Gecko/20100101 Firefox/30.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_2) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/537.75.14",
            "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Win64; x64; Trident/6.0)",
        };

        private static readonly Random Random = new();

        public static async Task<string> GetJsonAsync(string url, string host = "", string referer = "")
        {
            var client = new HttpClient();
            if(!string.IsNullOrWhiteSpace(host)) client.DefaultRequestHeaders.Add("host", host);
            if(!string.IsNullOrWhiteSpace(referer)) client.DefaultRequestHeaders.Add("referer", referer);
            client.DefaultRequestHeaders.Add("user-Agent", UserAgent[Random.Next(UserAgent.Length)]);
            return await client.GetStringAsync(url);
        }
    }
}
