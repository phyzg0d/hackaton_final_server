using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;

namespace ServerAspNetCoreLinux.Commands
{
    public class UserConnectionCommand : ExecuteCommand
    {
        private string _userId { get; }
        private string _session { get; }

        public UserConnectionCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(UserConnectionCommand))
        {
            _userId = data["userId"];
            _session = data["session"];
        }

        public override void Execute(ServerContext context)
        {
            if (context.UserModel.Contains(_userId))
            {
                var user = context.UserModel.Get(_userId);
                
                UserParams.Add("permission", user.Properties.Get<string>("permission"));
                
                if (user.Properties.Get<string>("session").Value == _session)
                {
                    UserParams.Add("authorisation", true);
                    
                    ServerLoggerModel.Log(TypeLog.UserMessage, $"user {_userId} is authorized");
                }
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