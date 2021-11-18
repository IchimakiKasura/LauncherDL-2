namespace launcherDL
{
    class LauncherDL_fetcher
    {
        public async static Task<string> GetURLTitle(string url)
        {
            string title = string.Empty;
            try
            {
                var req = new HttpClient();
                var resp = await Task.Run(() => req.GetAsync(url).Result);
                var data = resp.Content.ReadAsStringAsync().Result;
                title = LauncherDL_regexClass.URLTitle.Match(data).Groups["title"].ToString();
                return title;   
            }
            catch
            {
                return "Error";
            }
        }
    }
}