using Npgsql;

public interface IDbConnectionFactory
{
    string GetConnectionString();
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        var username = Environment.GetEnvironmentVariable("DB_USERNAME");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
        {
            Username = username,
            Password = password
        };

        _connectionString = connectionStringBuilder.ConnectionString;
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }
}
