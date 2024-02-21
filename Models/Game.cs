namespace Models;

public class Game
{
    public Game(int id, int winner, int score)
    {
        Id = id;
        Winner = winner;
        Score = score;
    }

    public int Id { get; }
    public int Winner { get; }
    public int Score { get; }
    public List<Player>? Players { get; set; } // Added '?' to make the property nullable
}
