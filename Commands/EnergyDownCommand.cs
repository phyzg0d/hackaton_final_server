using System;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;

namespace ServerAspNetCoreLinux.Commands
{
    public class EnergyDownCommand : ExecuteCommand
    {
        private string _email { get; set; }
        private float _energyDown { get; set; }
        
        public EnergyDownCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(EnergyDownCommand))
        {
            _email = data["email"];
            _energyDown = (float) Convert.ToDouble(data["energy"]);
        }
        
        public override void Execute(ServerContext context)
        {
            var user = context.UserModel[_email];
            var energyLeft = user.Properties.Get<float>("hours_left");
            
            energyLeft.Value -= _energyDown;
            
            Send();
        }
    }
}       