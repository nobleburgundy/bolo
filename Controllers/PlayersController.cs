using Microsoft.AspNetCore.Mvc;
using Models;

namespace bolo.Controllers;

[ApiController]
[Route("api/players")]
public class PlayersController : ControllerBase
{
    private readonly IRepository<Player> _playersRepository;

    public PlayersController(IRepository<Player> playersRepository)
    {
        _playersRepository = playersRepository;
    }

    [HttpGet]
    public IEnumerable<Player> GetAll()
    {
        return _playersRepository.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Player> GetById(int id)
    {
        var player = _playersRepository.GetById(id);
        if (player == null)
        {
            return NotFound();
        }
        return player;
    }

    [HttpPost]
    public ActionResult<Player> Create(Player player)
    {
        _playersRepository.Add(player);
        return CreatedAtAction(nameof(GetById), new { id = player.Id }, player);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Player player)
    {
        if (id != player.Id)
        {
            return BadRequest();
        }
        _playersRepository.Update(player);
        return NoContent();
    }
}
