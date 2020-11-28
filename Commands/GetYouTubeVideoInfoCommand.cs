using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;
using YoutubeExtractor;

namespace ServerAspNetCoreLinux.Commands
{
    public class GetYouTubeVideoInfoCommand : ExecuteCommand
    {
        private string _link { get; set; }
        
        public GetYouTubeVideoInfoCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(GetYouTubeVideoInfoCommand))
        {
            _link = data["link"];
        }

        public GetYouTubeVideoInfoCommand(string link) : base(null, null, nameof(GetYouTubeVideoInfoCommand))
        {
            _link = link;
        }

        public override void Execute(ServerContext context)
        {
            // var videoInfos = DownloadUrlResolver.GetDownloadUrls(_link);
            // var video = videoInfos.Where(info => info.CanExtractAudio)
            //     .OrderByDescending(info => info.AudioBitrate)
            //     .First();
            //
            // if (video.RequiresDecryption){DownloadUrlResolver.DecryptDownloadUrl(video);}
            //
            // var audioDownloader = new AudioDownloader(video, Path.Combine("C:/Users/mephy/OneDrive/Рабочий стол/YOU", video.Title + video.AudioExtension));
            //
            // audioDownloader.Execute();
            //
            // Send();
        }
    }
}