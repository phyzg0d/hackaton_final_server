using CoreServer;
using CoreServer.Replication.Replication;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.ServerCore;
using ServerAspNetCoreLinux.ServerCore.Commands;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;
using ServerAspNetCoreLinux.ServerCore.Utilities;
using ServerAspNetCoreLinux.Users;
using static ServerAspNetCoreLinux.Replication.DbConst;

namespace ServerAspNetCoreLinux.Core
{
    public class StartController
    {
        private readonly ServerContext _context;
        private ControllerCollection _controllerCollection = new ControllerCollection();
        private HttpContext _httpContext;

        public StartController(ServerContext context, HttpContext httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            
            _context.ReplicationCollection = new ReplicationModelCollection();
            
            var dataBaseConnection= new DataBaseConnection();
            _context.DataBaseConnection = dataBaseConnection;
            dataBaseConnection.Connect();
                
            if (!context.DataBaseConnection.IsConnection)
            {
                ServerLoggerModel.Log(TypeLog.Fatal, "StartController: the server did not start correctly due to problems with the connection to the database");
            }
            else
            {
                CreateModels();
                CreateControllers();
                _controllerCollection.Activate();
                
                new Deserializer().Deserialize(context);
                
                ServerLoggerModel.Log(TypeLog.Info, "server started");
            }
        }

        private void CreateModels()
        {
            _context.CommandModel = new CommandModel();
            _context.Factory = new Factory(_context);
            _context.UserModel = new UserModel(new SerializerConfig(UserInsertCommand, UserUpdateCommand), new DeserializerConfig(UserSelectCommand));
            
            _context.ReplicationCollection.Add(_context.UserModel);
        }

        private void CreateControllers()
        {
            _controllerCollection.Add(new CommandController(_context, _context.CommandModel));
            _controllerCollection.Add(new ReplicationController(_context));
        }
    }
}