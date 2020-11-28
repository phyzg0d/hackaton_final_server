using System.Collections.Generic;
using System.Data.SqlClient;
using CoreServer.Replication.Replication;

namespace ServerAspNetCoreLinux.ServerCore
{
    public class Deserializer
    {
        public void Deserialize(ServerContext context)
        {
            foreach (var model in context.ReplicationCollection.Get())
            {
                var command = new SqlCommand("")
                {
                    Connection = context.DataBaseConnection.Connection
                };

                command.CommandText = model.DeserializerConfig.CommandSelect;
                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        model.Deserialize(dataReader);
                    }
                }
                dataReader.Close();
            }
        }
    }
}