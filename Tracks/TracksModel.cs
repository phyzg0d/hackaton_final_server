using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CoreServer.Replication.Replication;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;
using ServerAspNetCoreLinux.Users;

namespace ServerAspNetCoreLinux.Tracks
{
    public class TracksModel : ReplicationModel
    {
        private Dictionary<string, TrackUnitModel> _tracks = new Dictionary<string, TrackUnitModel>();

        public TracksModel(DeserializerConfig deserializerConfig) : base(null, deserializerConfig, "hackaton_tracks")
        {
        }

        public override void Deserialize(SqlDataReader dataReader)
        {
            var track = new TrackUnitModel(dataReader);

            if (!_tracks.ContainsKey(track.TrackName))
            {
                _tracks.Add(track.TrackName, track);
            }
            
            // ServerLoggerModel.Log(TypeLog.Info,"track deserialize was completed");
        }
    }
}