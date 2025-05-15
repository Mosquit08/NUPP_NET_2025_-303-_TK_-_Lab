namespace Library.Infrastructure.Configuration
{
    public class DatabaseSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }

        public string GetConnectionString()
        {
            return $"Server={Server};Port={Port};Database={Database};User={User};Password={Password};";
        }
    }
} 