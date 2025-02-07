using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

public class MatchesApiController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchesApiController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    // Ваши методы API, такие как Get, Post, Put, Delete
}
