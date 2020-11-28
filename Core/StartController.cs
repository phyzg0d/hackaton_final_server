using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using CoreServer;
using CoreServer.Replication.Replication;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.Commands;
using ServerAspNetCoreLinux.ServerCore;
using ServerAspNetCoreLinux.ServerCore.Commands;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;
using ServerAspNetCoreLinux.ServerCore.Utilities;
using ServerAspNetCoreLinux.Tracks;
using ServerAspNetCoreLinux.Users;
using YoutubeExtractor;
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

            var dataBaseConnection = new DataBaseConnection();
            _context.DataBaseConnection = dataBaseConnection;
            dataBaseConnection.Connect();

            if (!context.DataBaseConnection.IsConnection)
            {
                ServerLoggerModel.Log(TypeLog.Fatal, "StartController: the server did not start correctly due to problems with the connection to the database");
            }
            else
            {
                var link = "https://www.youtube.com/watch?v=1RxtNtRiOqQ&ab_channel=MaxKorzh";
                var p = new Process {StartInfo = {FileName = "youtube-dl", ArgumentList = {"-o", "hackaton_test1.m4a", "-f", "140", link, "--exec", "mv {} /root/hackaton_final_server/test/"}}};
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                p.WaitForExit();

                var p2 = new Process {StartInfo = {FileName = "ffmpeg", ArgumentList = {"-i", "hackaton_test1.m4a", "-ss", "00:01:52", "-c", "copy", "-t", "00:00:10", "hackaton_test_output.mp4"}}};
                p2.StartInfo.UseShellExecute = false;
                p2.StartInfo.RedirectStandardOutput = true;
                p2.Start();
                p2.WaitForExit();

                var data = File.ReadAllBytes("test/hackaton_test_output.mp4");
                var postParameters = new Dictionary<string, object>();
                
                postParameters.Add("url", new FileParameter(data, "file", "application/octet-stream"));
                postParameters.Add("api_token", "e1d593768b4a52f1f6229de45d64cd2d");
                postParameters.Add("return", "timecode,apple_music,deezer,spotify");

                var postURL = "https://api.audd.io/recognize";
                var userAgent = "Someone";

                var webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

                var stream = webResponse.GetResponseStream();

                var responseReader = new StreamReader(stream);

                var fullResponse = responseReader.ReadToEnd();

                webResponse.Close();
                Console.WriteLine(fullResponse);


                CreateModels();
                CreateControllers();
                _controllerCollection.Activate();

                new Deserializer().Deserialize(context);

                ServerLoggerModel.Log(TypeLog.Info, "server started");
            }
        }

        public static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    var securePassword = new SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }

        private void CreateModels()
        {
            _context.CommandModel = new CommandModel();
            _context.Factory = new Factory(_context);
            _context.UserModel = new UserModel(new SerializerConfig(UserInsertCommand, UserUpdateCommand), new DeserializerConfig(UserSelectCommand));
            _context.TracksModel = new TracksModel(new DeserializerConfig(TracksSelectCommand));

            _context.ReplicationCollection.Add(_context.UserModel);
        }

        private void CreateControllers()
        {
            _controllerCollection.Add(new CommandController(_context, _context.CommandModel));
            _controllerCollection.Add(new ReplicationController(_context));
        }
    }
}