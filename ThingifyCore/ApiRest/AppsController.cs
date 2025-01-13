using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using ThingifyCore.Models;
using ThingifyCore.BackgroundService;

namespace ThingifyCore.ApiRest;

[ApiController]
[Route("api-rest/apps")]
public class AppsController : ControllerBase
{
    private readonly ILogger<AppsController> _logger;

    public AppsController(
        ILogger<AppsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetApps")]
    public IEnumerable<object> Get()
    {
        return new object[] {
            new string[] { "boe" , "bah" },
            new Dictionary<string, string> { { "key1", "val1" }, { "key2", "val2" } }
        };
    }
}
