using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;

namespace ServerAspNetCoreLinux.Commands.SignIn_SignOut
{
    public class SignOutCommand : ExecuteCommand
    {
        private string _userId { get; }

        public SignOutCommand(IFormCollection data, HttpContext httpContext) : base(data,httpContext, nameof(SignOutCommand))
        {
            _userId = data["userId"];
        }

        public override void Execute(ServerContext context)
        {
            var user = context.UserModel.Get(_userId);
            
            user.Properties.Get<string>("session").Value = string.Empty;
            user.Properties.Get<int>("is_authorisation").Value = 0;
            
            UserParams.Add("authorisation", false);
            
            Send();
            ServerLoggerModel.Log(TypeLog.UserMessage, $"User {_userId} logged out");
        }
    }
}