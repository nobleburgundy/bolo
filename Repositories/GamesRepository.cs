using System;
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
        List<Game> games = new List<Game>();
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (var command = new NpgsqlCommand("SELECT * FROM games", conn))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Game game = new Game(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("winner")),
                            reader.GetDateTime(reader.GetOrdinal("game_date"))
                        );

                        games.Add(game);
                    }
                }
            }
        }

        return games;
    }

    public Game GetById(int id)
    {
        var game = new Game(0, 0, new DateTime());

        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM Games WHERE Id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        game = new Game(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("winner")),
                            reader.GetDateTime(reader.GetOrdinal("game_date"))
                        );
                    }
            }
        }

        return game;
    }

    public void Update(Game entity)
    {
        throw new NotImplementedException();
    }
}
