using DeveCoolLib.Status;
using Microsoft.AspNetCore.Mvc;

namespace UnityGameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;

        public StatusController(ILogger<StatusController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<StatusModel> GetAsync()
        {
            _logger.Log(LogLevel.Information, "### Status Controller Get() called");

            var statusModel = StatusObtainer.GetStatus();

            return statusModel;
        }
    }
}
