using System.Collections.Generic;
using CoreServer.Replication.Property;
using CoreServer.Replication.Replication;
using ServerAspNetCoreLinux.ServerCore.Utilities;

namespace ServerAspNetCoreLinux.Tracks
{
    public class TracksInterprice : IReplicationUnitModel
    {
        public PropertyCollection Properties { get; } = new PropertyCollection();
        public bool IsNew { get; set; }

        public TracksInterprice(Dictionary<string, object> data)
        {
            Properties._properties = new Dictionary<string, IProperty>();
            Properties.Create<string>("artist", true).Value = data.GetString("artist");
            Properties.Create<string>("title", true).Value = data.GetString("title");
            Properties.Create<string>("album", true).Value = data.GetString("album");
            Properties.Create<string>("release_date", true).Value = data.GetString("release_date");
            Properties.Create<string>("song_link", true).Value = data.GetString("song_link");
            Properties.Create<string>("label", true).Value = data.GetString("label");
            Properties.Create<string>("image_url", true).Value = data.GetNode("artwork").GetString("url");;
        }
    }
}