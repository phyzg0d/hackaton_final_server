using System;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;
using ServerAspNetCoreLinux.Users;

namespace ServerAspNetCoreLinux.Commands.Registration
{
    public class RegistrationCommand : ExecuteCommand
    {
        private string _email { get; }
        private string _password { get; }
        private string _login { get; }
        private string _permission { get; }
        
        private float _money { get; }
        private float _hours_left { get; }

        public RegistrationCommand(IFormCollection data, HttpContext httpContext) : base(data,httpContext, nameof(RegistrationCommand))
        {
            _email = data["email"];
            _password = data["password"];
            _login = data["login"];
            _permission = data["permission"];
            _money = (float) Convert.ToDouble(data["money"]);
            _hours_left = (float) Convert.ToDouble(data["hours_left"]);
        }

        public override void Execute(ServerContext context)
        {
            var userParam = new Random().Next(0, 100000).ToString();
            
            // UserParams.Add("userId", string.Empty);
            // UserParams.Add("session", string.Empty);

            if (context.UserModel.Emails.ContainsKey(_email))
            {
                UserParams["error"] = true;
                UserParams["error_text"] = "Email exist";
            }
            else
            {
                // UserParams["userId"] = userParam;
                // UserParams["session"] = userParam;
                
                var user = new UserUnitModel(_email, _password, _login, _money, _permission, _hours_left);
                
                context.UserModel.Add(user);
                ServerLoggerModel.Log(TypeLog.UserMessage, $"user {userParam} registered");
            }
            Send();
        }
    }
}