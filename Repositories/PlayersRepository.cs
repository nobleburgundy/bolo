using Npgsql;

namespace Repositories
{
    public class PlayersRepository : IRepository<Player>
    {
        private readonly string _connectionString;

        public PlayersRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connectionString = dbConnectionFactory.GetConnectionString();
        }

        public void Add(Player entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.LastName == null || entity.FirstName == null)
                throw new ArgumentNullException("First name and last name cannot be null");

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO players (firstname, lastname) VALUES (@firstname, @lastname)", connection))
                {
                    command.Parameters.AddWithValue("firstname", entity.FirstName);
                    command.Parameters.AddWithValue("lastname", entity.LastName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Player> GetAll()
        {
            var players = new List<Player>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM players", connection))
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        players.Add(new Player(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            }

            return players;
        }

        public Player GetById(int id)
        {
            var player = new Player(0, "", "");

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM players WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                            player = new Player(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                }
            }

            return player;
        }

        public void Update(Player entity)
        {
            throw new NotImplementedException();
        }
    }
}
