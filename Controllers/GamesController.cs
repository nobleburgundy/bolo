using Microsoft.AspNetCore.Mvc;

namespace bolo.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private static readonly string[] Players = new[]
    {
        "James", "Laura", "Luther", "Sonia", "Andrew", "Raquel", "Bob"
    };

    private readonly ILogger<GamesController> _logger;

    public GamesController(ILogger<GamesController> logger)
    {
        _logger = logger;
        _logger.LogInformation("pastGames called");
    }

    [HttpGet]
    public IEnumerable<BoloGame> Get()
    {
        Console.WriteLine("pastGames called");
        _logger.LogWarning("pastGames called");
        return Enumerable.Range(1, 5).Select(index => new BoloGame
        {
            Date = DateTime.Now.AddDays(-index),
            Winner = Players[Random.Shared.Next(Players.Length)],
            Players = Players.Take(Random.Shared.Next(4, Players.Length)).ToArray()
        })
        .ToArray();
    }
}
