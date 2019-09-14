using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Parser
    {
        public static async Task<string> getSite(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}
