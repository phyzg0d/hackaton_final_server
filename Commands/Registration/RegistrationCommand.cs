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

        public RegistrationCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(RegistrationCommand))
        {
            _email = data["email"];
            _password = data["password"];
            _login = data["login"];
        }

        public override void Execute(ServerContext context)
        {
            try
            {
                if (context.UserModel.Emails.ContainsKey(_email))
                {
                    UserParams["error"] = true;
                    UserParams["error_text"] = "Email exist";
                }
                else
                {
                    var user = new UserUnitModel(_email, _password, _login, 0, "user", 2);

                    user.IsNew = true;
                    
                    context.UserModel.Add(user);
                    ServerLoggerModel.Log(TypeLog.UserMessage, $"user {_login} registered");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Send();
        }
    }
}