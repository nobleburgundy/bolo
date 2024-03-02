public class Player
{
    public Player(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public Player(int id)
    {
        Id = id;
    }

    public int Id { get; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
