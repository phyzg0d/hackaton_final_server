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
        private Dictionary<string, TrackUnitModel> _tracksAll = new Dictionary<string, TrackUnitModel>();
        private Dictionary<string, TrackUnitModel> _tracksFree = new Dictionary<string, TrackUnitModel>();
        private Dictionary<string, TrackUnitModel> _tracksLicensed = new Dictionary<string, TrackUnitModel>();

        public TracksModel(DeserializerConfig deserializerConfig) : base(null, deserializerConfig, "hackaton_tracks")
        {
        }

        public override void Deserialize(SqlDataReader dataReader)
        {
            var track = new TrackUnitModel(dataReader);

            if (!_tracksAll.ContainsKey(track.TrackName))
            {
                _tracksAll.Add(track.TrackName, track);
            }

            // if (!_tracksFree.ContainsKey(track.TrackName))
            // {
            //     if (track.RightsStatus == "FREE")
            //     {
            //         for (var i = 0; i < 50; i++)
            //         {
            //             _tracksFree.Add(track.TrackName, track);
            //         }
            //     }
            // }
            //
            // if (!_tracksLicensed.ContainsKey(track.TrackName))
            // {
            //     if (track.RightsStatus == "DMCA")
            //     {
            //         for (var i = 0; i < 150; i++)
            //         {
            //             _tracksLicensed.Add(track.TrackName, track);
            //         }
            //     }
            // }

            // ServerLoggerModel.Log(TypeLog.Info,"track deserialize was completed");
        }
    }
}