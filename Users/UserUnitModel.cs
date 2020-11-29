using System.Collections.Generic;
using CoreServer.Replication.Replication;
using ServerAspNetCoreLinux.Tracks;

namespace ServerAspNetCoreLinux.Users
{
    public class UserUnitModel : ReplicationData
    {
        public string Email
        {
            get => Properties.Get<string>("email").Value;
            set => Properties.Get<string>("email").Value = value;
        }
        public string Password
        {
            get => Properties.Get<string>("password").Value;
            set => Properties.Get<string>("password").Value = value;
        }
        public string Login
        {
            get => Properties.Get<string>("id").Value;
            set => Properties.Get<string>("id").Value = value;
        }
        public float Money
        {
            get => Properties.Get<float>("money").Value;
            set => Properties.Get<float>("money").Value = value;
        }
        public string Permission
        {
            get => Properties.Get<string>("permission").Value;
            set => Properties.Get<string>("permission").Value = value;
        }
        public float HoursLeft
        {
            get => Properties.Get<float>("hours_left").Value;
            set => Properties.Get<float>("hours_left").Value = value;
        }
        public List<object> History = new List<object>();
        
        public UserUnitModel(string email, string password, string login, float money, string permission, float hoursLeft)
        {
            Properties.Create<string>("email", true).Value = email;
            Properties.Create<string>("password", true).Value = password;
            Properties.Get<string>("id").Value = login;
            Properties.Create<float>("money", true).Value = money;
            Properties.Create<string>("permission", true).Value = permission;
            Properties.Create<float>("hours_left", true).Value = hoursLeft;
        }
        
        public override void Deserialize(IDictionary<string, object> data)
        {
        }
    }
}