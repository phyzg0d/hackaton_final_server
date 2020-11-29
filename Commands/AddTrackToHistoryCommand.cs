using System;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;

namespace ServerAspNetCoreLinux.Commands
{
    public class AddTrackToHistoryCommand : ExecuteCommand
    {
        private string _track { get; set; }
        public AddTrackToHistoryCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(AddTrackToHistoryCommand))
        {
            _track = data["track"];
            Console.WriteLine("ADDED");
        }

        public override void Execute(ServerContext context)
        {
            Send();
        }
    }
}