using System;
using Microsoft.Extensions.Logging;
using Models;
using Npgsql;

namespace Repositories;

public class GamesRepository : IRepository<Game>
{
    private readonly string _connectionString;
    private PlayersRepository playersRepository;

    public GamesRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _connectionString = dbConnectionFactory.GetConnectionString();
        playersRepository = new PlayersRepository(dbConnectionFactory);
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

                        game.Players = GetGamePlayers(game.Id);

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
        List<PlayerScore> players = new List<PlayerScore>();

        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            using (
                var cmd = new NpgsqlCommand(
                    "SELECT * FROM game_players WHERE game_id = @gameId",
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
                                reader.GetInt32(reader.GetOrdinal("player_id")),
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
        var gameId = NewGame(gameDate);
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            foreach (var gamePlayer in gamePlayers)
            {
                var playerId = playersRepository.GetById(gamePlayer.Id);
                if (playerId.Id == 0)
                {
                    Console.WriteLine($"Player not found: {gamePlayer.FirstName} {gamePlayer.LastName}");
                    // TODO if new player not found, do we want to create it here?
                    // gamePlayer.PlayerId = playersRepository.Add(new Player(gamePlayer.FirstName, gamePlayer.LastName));
                    // Console.WriteLine($"AddGamesWithPlayersScores new playerId: {gamePlayer.PlayerId}");
                    throw new ArgumentException("Player not found");
                }


                using (
                    var cmd = new NpgsqlCommand(
                        "INSERT INTO game_players (game_id, player_id, score) VALUES (@gameId, @playerId, @score) returning game_id",
                        conn
                    )
                )
                {
                    cmd.Parameters.AddWithValue("gameId", gameId);
                    cmd.Parameters.AddWithValue("playerId", gamePlayer.Id);
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
                    "INSERT INTO game_players (game_id, player_id, score) VALUES (@gameId, @playerId, @score)",
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

    public int Add(Game entity)
    {
        throw new NotImplementedException();
    }
}
