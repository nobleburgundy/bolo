using System.Text.Json.Serialization;

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

    [JsonConstructor]
    public Player(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
