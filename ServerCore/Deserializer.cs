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
            
            var command1 = new SqlCommand("")
            {
                Connection = context.DataBaseConnection.Connection
            };

            command1.CommandText = context.TracksModel.DeserializerConfig.CommandSelect;
            var dataReader1 = command1.ExecuteReader();

            if (dataReader1.HasRows)
            {
                while (dataReader1.Read())
                {
                    context.TracksModel.Deserialize(dataReader1);
                }
            }
            
            dataReader1.Close();
        }
    }
}