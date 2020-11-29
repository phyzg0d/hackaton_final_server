using System;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.Lib.fastJSON;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;
using ServerAspNetCoreLinux.Users;

namespace ServerAspNetCoreLinux.Commands
{
    public class UserConnectionCommand : ExecuteCommand
    {
        private string _userId { get; }

        public UserConnectionCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(UserConnectionCommand))
        {
            _userId = data["user_id"];
        }

        public override void Execute(ServerContext context)
        {
            if (context.UserModel.Contains(_userId))
            {
                var user = context.UserModel.Get(_userId);

                UserParams.Add("authorisation", true);
                UserParams.Add("user", user.Properties.GetSerialize());
                UserParams.Add("history", JSON.ToJSON(((UserUnitModel)user).History));

                ServerLoggerModel.Log(TypeLog.UserMessage, $"user {_userId} is authorized");
            }
            else
            {
                UserParams["authorisation"] = false;

                ServerLoggerModel.Log(TypeLog.UserMessage, "user authorization interrupted");
            }

            Send();
        }
    }
}