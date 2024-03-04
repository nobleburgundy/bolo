using System;

namespace Models;

public class GamePlayersScores
{
    public IEnumerable<PlayerScore> PlayerScores { get; set; }
    public DateTime gameDate { get; set; }
    public Game? game {get;set;}

    public GamePlayersScores(IEnumerable<PlayerScore> playerScores)
    {
        PlayerScores = playerScores;
    }
}
