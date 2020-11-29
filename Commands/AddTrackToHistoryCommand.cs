using System;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;

namespace ServerAspNetCoreLinux.Commands
{
    public class AddTrackToHistoryCommand : ExecuteCommand
    {
        private string _track { get; set; }
        private string _userId { get; set; }
        public AddTrackToHistoryCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(AddTrackToHistoryCommand))
        {
            _track = data["track"];
            _userId = data["user_id"];
        }

        public override void Execute(ServerContext context)
        {
            context.UserModel[_userId].Properties.GetDictionary<string>("history").Value.Add(_track, "");
            Send();
        }
    }
}