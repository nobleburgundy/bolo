using Microsoft.AspNetCore.Mvc;
using Models;

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

    // Other actions...
}
