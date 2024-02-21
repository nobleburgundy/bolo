public class Player
{
    public Player(int id, string firstName, string lastName, int score)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Score = score;
    }

    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public int Score { get; }
}
