using Npgsql;

namespace Repositories;

public class PlayersRepository : IRepository<Player>
{
    private readonly string _connectionString;

    public PlayersRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _connectionString = dbConnectionFactory.GetConnectionString();
    }

    public void Add(Player entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Player> GetAll()
    {
        var players = new List<Player>();

        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM Players", conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    players.Add(new Player(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3)));
        }

        return players;
    }

    public Player GetById(int id)
    {
        // todo: fix player ctor
        var player = new Player(0, "", "", 0);

        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM Players WHERE Id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        player = new Player(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
            }
        }

        return player;
    }

    public void Update(Player entity)
    {
        throw new NotImplementedException();
    }
}
