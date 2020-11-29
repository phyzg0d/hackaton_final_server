using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;
using ServerAspNetCoreLinux.Users;

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
            ((UserUnitModel)context.UserModel[_userId]).History.Add(_track);
            Send();
        }
    }
}