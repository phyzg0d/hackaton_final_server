using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CoreServer.Replication.Replication;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;

namespace ServerAspNetCoreLinux.Users
{
    public class UserModel : ReplicationModel
    {
        private Dictionary<string, IReplicationData> _users = new Dictionary<string, IReplicationData>();
        public Dictionary<string, string> Emails = new Dictionary<string, string>();

        public UserModel(SerializerConfig serializerConfig, DeserializerConfig deserializerConfig) : base(serializerConfig, deserializerConfig, "users")
        {
        }

        public override void Deserialize(SqlDataReader dataReader)
        {
            var user = new UserUnitModel(dataReader.GetString("email"), dataReader.GetString("password"), dataReader.GetString("id"), Convert.ToSingle(dataReader.GetString("money")), dataReader.GetString("permission"), Convert.ToSingle(dataReader.GetString("hours_left")));
            Add(user);
            ServerLoggerModel.Log(TypeLog.Info, "users deserialize was completed");
        }

        public override void Add(IReplicationData user)
        {
            base.Add(user);
            Emails.Add(user.Properties.Get<string>("email").Value, user.Properties.Get<string>("id").Value);
        }
    }
}