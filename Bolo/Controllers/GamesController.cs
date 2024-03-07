using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;

namespace bolo.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly IRepository<Game> _gamesRepository;

    public GamesController(IRepository<Game> gamesRepository)
    {
        _gamesRepository = gamesRepository;
    }

    [HttpGet]
    public IEnumerable<Game> GetAll()
    {
        return _gamesRepository.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Game> GetById(int id)
    {
        var game = _gamesRepository.GetById(id);

        if (game == null)
        {
            return NotFound();
        }

        var players = GetGamePlayers(id);

        game.Players = players;
        return game;
    }

    [HttpGet("{id}/players")]
    public IEnumerable<PlayerScore> GetGamePlayers(int id)
    {
        var gamesRepository = (GamesRepository)_gamesRepository;
        return gamesRepository.GetGamePlayers(id);
    }

    [HttpPost]
    public ActionResult<Game> Create(GamePlayersScores gamePlayers)
    {
        var gamesRepository = (GamesRepository)_gamesRepository;

        var newGameId = gamesRepository.AddGameWithPlayerScores(gamePlayers.gameDate, gamePlayers.PlayerScores);
        return CreatedAtAction(nameof(GetById), new { id = newGameId }, newGameId);
    }

    [HttpPost("{id}/addPlayers")]
    public ActionResult<PlayerGame> AddPlayersToGame(IEnumerable<PlayerGame> gamePlayers)
    {
        Console.WriteLine("gamePlayers: " + gamePlayers.Count());

        var gamesRepository = (GamesRepository)_gamesRepository;
        gamesRepository.AddPlayers(gamePlayers);
        return CreatedAtAction(nameof(GetGamePlayers), new { id = gamePlayers.First().GameId }, gamePlayers);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Game game)
    {
        if (id != game.Id)
        {
            return BadRequest();
        }
        _gamesRepository.Update(game);
        return NoContent();
    }
}
