using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using ThingifyCore.Models;
using ThingifyCore.BackgroundService;
using System;
using System.Threading.Tasks;

namespace ThingifyCore.ApiRest;

[ApiController]
[Route("api-rest/apps")]
public class AppsController : ControllerBase
{
    private readonly ILogger<AppsController> _logger;
    private readonly PodmanService _podmanService;

    public AppsController(
        ILogger<AppsController> logger,
        PodmanService podmanService)
    {
        _logger = logger;
        _podmanService = podmanService;
    }

    [HttpGet(Name = "GetApps")]
    public Task<string> Get()
    {
        return _podmanService.ListImages();
    }
}
