using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexPoc.VideoIndexerClient.ApiAccess
{
    public class VideoDownload : IVideoDownload
    {
        public async Task<string>GetVideoDownloadUrl(string accountId, string videoId, string accessToken, string region)
        {
            using HttpClient client = new HttpClient();
            
            var response = await client.GetAsync($"https://api.videoindexer.ai/{region}/Accounts/{accountId}/Videos/{videoId}/SourceFile/DownloadUrl?accessToken={accessToken}");


            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Failed to get video Url");
            }
        }

    }
}
