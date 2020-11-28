using CoreServer;
using ServerAspNetCoreLinux.Core;
using ServerAspNetCoreLinux.ServerCore;
using ServerAspNetCoreLinux.ServerCore.Commands;
using ServerAspNetCoreLinux.ServerCore.Utilities;
using ServerAspNetCoreLinux.Tracks;
using ServerAspNetCoreLinux.Users;

namespace ServerAspNetCoreLinux
{
    public class ServerContext : BaseServerContext
    {
        public CommandModel CommandModel;
        public Factory Factory;
        public UserModel UserModel;
        public TracksModel TracksModel;
    }
}