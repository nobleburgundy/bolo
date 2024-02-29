using System;

namespace Models;

public class Game
{
    public Game(int id, int winner, DateTime gameDate)
    {
        Id = id;
        Winner = winner;
        Game_Date = gameDate;
    }

    public int Id { get; set;}
    public int Winner { get; set;}
    public DateTime Game_Date { get; set;}
}
