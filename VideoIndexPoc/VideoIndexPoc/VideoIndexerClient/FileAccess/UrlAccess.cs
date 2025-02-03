using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexPoc.VideoIndexerClient.FileAccess
{
    public class UrlAccess : IUrlAccess
    {
        public async Task DownloadVideoAsync(string downloadUrl, string outputPath)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = System.IO.File.Create(outputPath);
            await stream.CopyToAsync(fileStream);
        }
    }
}
