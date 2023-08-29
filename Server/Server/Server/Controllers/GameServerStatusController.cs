using Microsoft.AspNetCore.Mvc;
using Server.Manager;

namespace Server.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class GameServerStatusController : ControllerBase
    {
        private readonly GameServerManager _gameServerManager;

        public GameServerStatusController(GameServerManager gameServerManager) 
        {
            this._gameServerManager = gameServerManager;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var allUsers = this._gameServerManager.GetAllUsers();

            return Ok(allUsers);
        }

        [HttpGet("test")]
        public IActionResult TestGetJson()
        {
            return Ok(new string[]{ "String1", "String2", "String3", "String4", "String5" });
        }
    }
}
