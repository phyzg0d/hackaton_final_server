using System.Data;
using System.Data.SqlClient;

namespace ServerAspNetCoreLinux.Tracks
{
    public class TrackUnitModel
    {
        public string AuthorName { get; set; }
        public string TrackName { get; set; }
        public string AlbumName { get; set; }
        public string LabelName { get; set; }
        public string TrackDuration { get; set; }
        public string RightsStatus { get; set; }
        public string DownloadLink { get; set; }
        public string TrackGenre { get; set; }
        
        public TrackUnitModel(SqlDataReader dataReader)
        {
            AuthorName = dataReader.GetString("author_name");
            TrackName = dataReader.GetString("name");
            AlbumName = dataReader.GetString("album_name");
            RightsStatus = dataReader.GetString("rights_status");

            // if (dataReader.GetString("label") != null)
            // {
            //     DownloadLink = dataReader.GetString("label");
            // }
            //
            // if (dataReader.GetString("duration") != null)
            // {
            //     DownloadLink = dataReader.GetString("duration");
            // }

            // if (dataReader.GetString("download_link") != null)
            // {
            //     DownloadLink = dataReader.GetString("download_link");
            // }
            
            // if (dataReader.GetString("genre") != null)
            // {
            //     TrackGenre = dataReader.GetString("genre");
            // }
        }
    }
}