using Microsoft.AspNetCore.Mvc;
using UnityGameServer.Hubs;

namespace UnityGameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HubsStatsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<HubsStatsController> _logger;

        public HubsStatsController(ILogger<HubsStatsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Room")]
        public IEnumerable<KeyValuePair<string, Room>> Get()
        {
            return UltraHub.Rooms.ToList();
        }
    }
}