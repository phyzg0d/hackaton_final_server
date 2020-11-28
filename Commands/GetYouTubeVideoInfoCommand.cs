using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using RestSharp;
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
            try
            {
                var random = new Random().Next(10000);
                var p = new Process {StartInfo = {FileName = "youtube-dl", ArgumentList = {"-o", $"{random}.m4a", "-f", "140", _link, "--exec", "mv {} test/"}}};
                p.Start();
                p.WaitForExit();

                var p2 = new Process {StartInfo = {FileName = "ffmpeg", ArgumentList = {"-i", $"test/{random}.m4a", "-ss", "00:01:52", "-c", "copy", "-t", "00:00:10", $"test/{random}_output.mp4"}}};
                p2.Start();
                p2.WaitForExit();

                var data = File.ReadAllBytes($"test/{random}_output.mp4");
                var postParameters = new Dictionary<string, object>();

                postParameters.Add("file", new FileParameter(data, "file", "application/octet-stream"));
                postParameters.Add("api_token", "e1d593768b4a52f1f6229de45d64cd2d");
                postParameters.Add("return", "timecode,apple_music,deezer,spotify");

                var postURL = "https://api.audd.io/recognize";
                var userAgent = "Someone";
                var webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);
                var stream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(stream);
                var fullResponse = responseReader.ReadToEnd();

                webResponse.Close();
            
                UserParams.Add("callback", fullResponse);
                File.Delete($"test/{random}.m4a");
                File.Delete($"test/{random}_output.mp4");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            Send();
        }
    }
}