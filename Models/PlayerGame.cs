public class PlayerGame
{
    public int PlayerId { get; set; }
    public int GameId { get; set; }
    public int Score { get; set; }
}

public class PlayerScore
{
    public PlayerScore(int playerId, int score)
    {
        PlayerId = playerId;
        Score = score;
    }

    public int PlayerId { get; set; }
    public int Score { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
