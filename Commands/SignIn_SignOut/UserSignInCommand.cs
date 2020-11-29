using System;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;

namespace ServerAspNetCoreLinux.Commands.SignIn_SignOut
{
    public class UserSignInCommand : ExecuteCommand
    {
        private string _email { get; }
        private string _password { get; }

        public UserSignInCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(UserSignInCommand))
        {
            _email = data["email"];
            _password = data["password"];
        }

        public override void Execute(ServerContext context)
        {
            UserParams.Add("permission", string.Empty);

            try
            {
                if (!context.UserModel.Emails.ContainsKey(_email))
                {
                    UserParams["error"] = true;
                    UserParams["error_text"] = "Wrong email";

                    ServerLoggerModel.Log(TypeLog.UserMessage, "unsuccessful login attempt");
                }
                else
                {
                    var userId = context.UserModel.Emails[_email];
                    var user = context.UserModel.Get(userId);

                    if (_password == user.Properties.Get<string>("password").Value)
                    {
                        UserParams.Add("user",user.Properties.GetSerialize());
                        
                        ServerLoggerModel.Log(TypeLog.UserMessage, $"user {user.Properties.Get<string>("id").Value} logged in");
                    }
                    else
                    {
                        UserParams["error"] = true;
                        UserParams["error_text"] = "Wrong password";
                        
                        ServerLoggerModel.Log(TypeLog.UserMessage, $"user {user.Properties.Get<string>("id").Value} entered incorrect data");
                    }
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