using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
            var p = new Process {StartInfo = {FileName = "youtube-dl", ArgumentList = {"-o", "hackaton_test", "-f", "140", _link, "--exec", "mv {} ~/test/"}}};
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            p.WaitForExit();

            var p2 = new Process {StartInfo = {FileName = "ffmpeg", ArgumentList = {"-i", "hackaton_test", "-ss", "00:01:52", "-c", "copy", "-t", "00:00:10", "hackaton_test_output.mp4"}}};
            p2.StartInfo.UseShellExecute = false;
            p2.StartInfo.RedirectStandardOutput = true;
            p2.Start();
            p2.WaitForExit();

            var data = File.ReadAllBytes("~/test/");
            var postParameters = new Dictionary<string, object>();
            postParameters.Add("url", new FileParameter(data, "file", "application/octet-stream"));
            postParameters.Add("api_token", "e1d593768b4a52f1f6229de45d64cd2d");
            postParameters.Add("return", "timecode,apple_music,deezer,spotify");

            var postURL = "https://api.audd.io/recognize";
            var userAgent = "Someone";

            var webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

            var stream = webResponse.GetResponseStream();

            var responseReader = new StreamReader(stream);

            var fullResponse = responseReader.ReadToEnd();

            webResponse.Close();
            Console.WriteLine(fullResponse);
            // Send();
        }
    }
    
    public class FileParameter
    {
        public byte[] File { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }

    public static class FormUpload
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent,
            Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
            return PostForm(postUrl, userAgent, contentType, formData);
        }

        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            request.PreAuthenticate = true;
            request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            request.Headers.Add("Authorization",
                "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("USER" + ":" + "PASSWORD")));


            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter) param.Value;

                    string header = string.Format(
                        "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }
    }
}