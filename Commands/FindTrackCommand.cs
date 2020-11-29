using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;

namespace ServerAspNetCoreLinux.Commands
{
    public class FindTrackCommand : ExecuteCommand
    {
        private string _text { get; set; }
        
        public FindTrackCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(FindTrackCommand))
        {
            _text = data["text"];
        }

        public override void Execute(ServerContext context)
        {
            var datas = new List<object>();
            
            foreach (var track in context.TracksModel.TracksLicensed)
            {
                if (track.Key.Contains(_text))
                {
                    datas.Add(track.Key);
                }    
            }
            
            UserParams.Add("data", datas);
            
            Send();
        }
    }
}