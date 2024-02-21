using Models;
using Npgsql;

namespace Repositories;

public class GamesRepository : IRepository<Game>
{
    private readonly string _connectionString;

    public GamesRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _connectionString = dbConnectionFactory.GetConnectionString();
    }

    public void Add(Game entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Game> GetAll()
    {
        var games = new List<Game>();

        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM Games", conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    games.Add(new Game(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));
        }

        return games;
    }

    public Game GetById(int id)
    {
        var game = new Game(0, 0, 0);

        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM Games WHERE Id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        game = new Game(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
            }
        }

        return game;
    }

    public void Update(Game entity)
    {
        throw new NotImplementedException();
    }
}
