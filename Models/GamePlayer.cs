public class GamePlayer
{
    public GamePlayer(int gameId, int playerId)
    {
        GameId = gameId;
        PlayerId = playerId;
    }

    public int GameId { get; }
    public int PlayerId { get; }
}
