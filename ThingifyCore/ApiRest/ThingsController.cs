using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using ThingifyCore.Models;
using ThingifyCore.BackgroundService;

namespace ThingifyCore.ApiRest;

[ApiController]
[Route("api-rest/things")]
public class ThingsController : ControllerBase
{
    private readonly ILogger<ThingsController> _logger;
    
    private readonly IThingsRepository _thingsRepository;

    public ThingsController(
        ILogger<ThingsController> logger,
        IThingsRepository thingsRepository)
    {
        _logger = logger;
        _thingsRepository = thingsRepository;
    }

    [HttpGet(Name = "GetThings")]
    public IEnumerable<Thing> Get()
    {
        return _thingsRepository.Get();
    }

    [HttpPatch(Name = "UpsertThings")]
    public void Upsert(List<Thing> things)
    {
        _thingsRepository.Upsert(things);
    }
}
