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

    // Other actions...
}
