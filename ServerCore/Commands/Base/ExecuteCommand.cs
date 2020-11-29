using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;
using Sharpbrake.Client.Impl;

namespace ServerAspNetCoreLinux.ServerCore.Commands.Base
{
    public abstract class ExecuteCommand : IExecuteCommand
    {
        private readonly HttpContext _httpContext;
        public readonly HttpRequest Request;
        protected HttpResponse Response { get; }
        protected Dictionary<string, object> UserParams = new Dictionary<string, object>();
        public string NameCommand { get; set; }

        protected ExecuteCommand(IFormCollection data, HttpContext httpContext, string nameCommand)
        {
            _httpContext = httpContext;
            NameCommand = nameCommand;
            Request = httpContext.Request;
            Response = httpContext.Response;
            UserParams.Add("error", false);
            UserParams.Add("error_text", string.Empty);

            _httpContext.Response.OnStarting(() =>
            {
                _httpContext.Response.Headers.Add("Host", "http://93.95.97.122:8000");
                _httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                _httpContext.Response.Headers.Add("Large-Allocation", "0");
                _httpContext.Response.Headers.Add("Content-Range", "bytes */*");
                _httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                _httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Access-Token, X-Application-Name, X-Request-Sent-Time, X-Requested-With, Content-Type, Authorization, Accept");
                _httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                _httpContext.Response.Headers.Add("X-Requested-With", "XMLHttpRequest");
                _httpContext.Response.Headers.Add("Accept", "*/*");
                _httpContext.Response.Headers.Add("Retry-After", "4");
                _httpContext.Response.Headers.Add("Cross-Origin-Resource-Policy", "cross-origin");

                return Task.FromResult(0);
            });

            _httpContext.Response.StatusCode = 200;
            ServerLoggerModel.Log(TypeLog.Info, $"{NameCommand}: {Response.HttpContext.Connection.RemoteIpAddress}:{Response.HttpContext.Connection.RemotePort}\r");
        }

        protected async void Send()
        {
            try
            {
                var sendObject = JsonConvert.SerializeObject(UserParams);
                var byteArray = System.Text.Encoding.UTF8.GetBytes(sendObject);

                _httpContext.Request.ContentType = "application/x-www-form-urlencoded";
                _httpContext.Request.ContentLength = byteArray.Length;

                using (var dataSteam = _httpContext.Response.Body)
                {
                    dataSteam.Write(byteArray, 0, byteArray.Length);
                }

                await _httpContext.Response.CompleteAsync();
            }
            catch (Exception e)
            {
                ServerLoggerModel.Log(TypeLog.Fatal, $"Response interrupted: {e.Message}");
            }
        }

        public abstract void Execute(ServerContext context);
    }
}