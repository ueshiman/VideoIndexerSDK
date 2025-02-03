using System.Threading.Tasks;
using VideoIndexPoc2.VideoIndexerClient.Model;

namespace VideoIndexPoc2.VideoIndexerClient;

public interface IVideoIndexerClient
{
    //Task AuthenticateAsync();
    //void Authenticate();

    /// <summary>
    /// Get Information about the Account
    /// </summary>
    /// <param name="accountName"></param>
    /// <returns></returns>
    Task<Account> GetAccountAsync(string accountName);

     Account GetAccount(string accountName);

    /// <summary>
    /// Uploads a video and starts the video index. Calls the uploadVideo API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Upload-Video)
    /// </summary>
    /// <param name="videoUrl"> Link To Publicy Accessed Video URL</param>
    /// <param name="videoName"> The Asset name to be used </param>
    /// <param name="exludedAIs"> The ExcludeAI list to run </param>
    /// <param name="waitForIndex"> should this method wait for index operation to complete </param>
    /// <exception cref="Exception"></exception>
    /// <returns> Video Id of the video being indexed, otherwise throws excpetion</returns>
    Task<string> UploadUrlAsync(string videoUrl , string videoName, string exludedAIs = null, bool waitForIndex = false );

    /// <summary>
    /// Uploads a video and starts the video index. Calls the uploadVideo API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Upload-Video)
    /// </summary>
    /// <param name="filePath"> Link To Publicy Accessed Video URL</param>
    /// <param name="videoName"> The Asset name to be used </param>
    /// <param name="exludedAIs"> The ExcludeAI list to run </param>
    /// <param name="waitForIndex"> should this method wait for index operation to complete </param>
    /// <exception cref="Exception"></exception>
    /// <returns> Video Id of the video being indexed, otherwise throws excpetion</returns>
    Task<string> UploadFileAsync(string filePath, string videoName, string exludedAIs = null, bool waitForIndex = false);

    /// <summary>
    /// Calls getVideoIndex API in 10 second intervals until the indexing state is 'processed'(https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Index)
    /// </summary>
    /// <param name="videoId"> The video id </param>
    /// <exception cref="Exception"></exception>
    /// <returns> Prints video index when the index is complete, otherwise throws exception </returns>
    Task WaitForIndexAsync(string videoId);

    /// <summary>
    /// Searches for the video in the account. Calls the searchVideo API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Videos)
    /// </summary>
    /// <param name="videoId"> The video id </param>
    /// <returns> Prints the video metadata, otherwise throws excpetion</returns>
    Task GetVideoAsync(string videoId);

    Task<string> FileUploadAsync(string videoName,  string mediaPath, string exludedAIs = null);

    /// <summary>
    /// Calls the getVideoInsightsWidget API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Insights-Widget)
    /// </summary>
    /// <param name="videoId"> The video id </param>
    /// <returns> Prints the VideoInsightsWidget URL, otherwise throws exception</returns>
    Task GetInsightsWidgetUrlAsync(string videoId);

    /// <summary>
    /// Calls the getVideoPlayerWidget API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Player-Widget)
    /// </summary>
    /// <param name="videoId"> The video id </param>
    /// <returns> Prints the VideoPlayerWidget URL, otherwise throws exception</returns>
    Task GetPlayerWidgetUrlAsync( string videoId);
}