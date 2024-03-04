using System;
using Microsoft.Extensions.Logging;
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

    public int Add(DateTime gameDate)
    {
        var game = new Game(0, gameDate);
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (
                var cmd = new NpgsqlCommand(
                    "INSERT INTO games (game_date) VALUES (@gameDate)",
                    conn
                )
            )
            {
                cmd.Parameters.AddWithValue("gameDate", game.Game_Date);
                cmd.ExecuteNonQuery();
            }
        }

        return game.Id;
    }

    /**
     * This method is used to create a new game in the database.
     * 
     * @param gameDate The date of the game.
     * @return The id of the new game.
     */
    public int NewGame(DateTime gameDate) 
    {
        var game = new Game(gameDate);
        Console.WriteLine("NewGame gameDate: " + game.Game_Date);
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (
                var cmd = new NpgsqlCommand(
                    "INSERT INTO games (game_date) VALUES (@gameDate) returning id",
                    conn
                )
            )
            {
                cmd.Parameters.AddWithValue("gameDate", game.Game_Date);
                return (int)(cmd.ExecuteScalar() ?? 0);
            }
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    /**
     * This method is used to get all games from the database.
     * 
     * @return A list of all games.
     */
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
                            reader.GetDateTime(reader.GetOrdinal("game_date"))
                        );

                        games.Add(game);
                    }
                }
            }
        }

        return games;
    }

    /**
     * This method is used to get a game by its id.
     * 
     * @param id The id of the game.
     * @return The game with the given id.
     */
    public Game GetById(int id)
    {
        var game = new Game(0, new DateTime());

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
                            reader.GetDateTime(reader.GetOrdinal("game_date"))
                        );
                    }
            }
        }

        return game;
    }

    /**
     * This method is used to get all players that played in a game.
     * 
     * @param gameId The id of the game.
     * @return A list of all players that played in the game.
     */
    public IEnumerable<PlayerScore> GetGamePlayers(int gameId)
    {
        Console.WriteLine("GetGamePlayers");
        List<PlayerScore> players = new List<PlayerScore>();

        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (
                var cmd = new NpgsqlCommand(
                    "SELECT * FROM playergame WHERE game_id = @gameId",
                    conn
                )
            )
            {
                cmd.Parameters.AddWithValue("gameId", gameId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        players.Add(
                            new PlayerScore(
                                reader.GetInt32(reader.GetOrdinal("playerid")),
                                reader.GetInt32(reader.GetOrdinal("score"))
                            )
                        );
                    }
                }
            }
        }

        players.ForEach(player =>
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (
                    var cmd = new NpgsqlCommand("SELECT * FROM players WHERE id = @playerId", conn)
                )
                {
                    cmd.Parameters.AddWithValue("playerId", player.PlayerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            player.FirstName = reader.GetString(reader.GetOrdinal("firstname"));
                            player.LastName = reader.GetString(reader.GetOrdinal("lastname"));
                        }
                    }
                }
            }
        });

        return players;
    }

    /**
     * This method is used to add a game with player scores to the database.
     * 
     * @param gameDate The date of the game.
     * @param gamePlayers A list of players and their scores.
     * @return The id of the new game.
     */
    public int AddGameWithPlayerScores(DateTime gameDate, IEnumerable<PlayerScore> gamePlayers)
    {
        Console.WriteLine("AddGamesWithPlayersScores gameDate" + gameDate);
        var gameId = NewGame(gameDate);
        Console.WriteLine("AddGamesWithPlayersScores gameId" + gameId);
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            foreach (var gamePlayer in gamePlayers)
            {
                using (
                    var cmd = new NpgsqlCommand(
                        "INSERT INTO playergame (game_id, player_id, score) VALUES (@gameId, @playerId, @score)",
                        conn
                    )
                )
                {
                    cmd.Parameters.AddWithValue("gameId", gameId);
                    cmd.Parameters.AddWithValue("playerId", gamePlayer.PlayerId);
                    cmd.Parameters.AddWithValue("score", gamePlayer.Score);
                    cmd.ExecuteNonQuery();
                }
            }

            return gameId;
        }
    }

    public void AddPlayers(IEnumerable<PlayerGame> playerGames) 
    {
        foreach (var playerGame in playerGames)
        {
            AddPlayerToGame(playerGame);
        }
    }

    /**
     * This method is used to add a player to a game in the database.
     * 
     * @param playerGame The player and their score in the game.
     */
    public void AddPlayerToGame(PlayerGame playerGame)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (
                var cmd = new NpgsqlCommand(
                    "INSERT INTO playergame (game_id, player_id, score) VALUES (@gameId, @playerId, @score)",
                    conn
                )
            )
            {
                cmd.Parameters.AddWithValue("gameId", playerGame.GameId);
                cmd.Parameters.AddWithValue("playerId", playerGame.PlayerId);
                cmd.Parameters.AddWithValue("score", playerGame.Score);
                cmd.ExecuteNonQuery();
            }

            using (
                var cmd = new NpgsqlCommand(
                    "INSERT INTO gameplayers (gameid, playerid) VALUES (@gameId, @playerId)",
                    conn
                )
            )
            {
                cmd.Parameters.AddWithValue("gameId", playerGame.GameId);
                cmd.Parameters.AddWithValue("playerId", playerGame.PlayerId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Update(Game entity)
    {
        throw new NotImplementedException();
    }

    public void Add(Game entity)
    {
        throw new NotImplementedException();
    }
}
