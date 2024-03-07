using System;
using System.Collections.Generic;

namespace Models;

public class Game
{
    public Game(int id, DateTime gameDate)
    {
        Id = id;
        Game_Date = gameDate;
    }

    public Game(DateTime gameDate) {
        Game_Date = gameDate;
    }

    public int Id { get; set;}
    public int Winner { get; set;}
    public DateTime Game_Date { get; set;}
    public IEnumerable<PlayerScore>? Players { get; set; }
}
