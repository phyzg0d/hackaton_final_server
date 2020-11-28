namespace ServerAspNetCoreLinux.Replication
{
    public static class DbConst
    {
        public const string UserInsertCommand = "INSERT INTO hackaton_users(email, password, login, money, permission, hours_left) VALUES(@email, @password, @login, @money, @permission, @hours_left)";
        public const string UserUpdateCommand = "UPDATE hackaton_users SET hours_left = @hours_left WHERE email = @email";
        public const string UserSelectCommand = "SELECT * FROM hackaton_users";
    }
}