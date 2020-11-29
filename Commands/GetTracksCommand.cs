using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;

namespace ServerAspNetCoreLinux.Commands
{
    public class GetTracksCommand : ExecuteCommand
    {
        private string _pieceOfTrack { get; set; }
        
        public GetTracksCommand(IFormCollection data, HttpContext httpContext) : base(data, httpContext, nameof(GetTracksCommand))
        {
            _pieceOfTrack = data["piece"];
        }

        public override void Execute(ServerContext context)
        {
            Send();
        }
    }
}