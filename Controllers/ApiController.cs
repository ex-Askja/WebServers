using Microsoft.AspNetCore.Mvc;
using WebServers.Domain.Interfaces;

namespace WebServers.Controllers
{
    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IServerList _servers;

        public ApiController(ILogger<ApiController> logger, IServerList servers)
        {
            _logger = logger;
            _servers = servers;
        }

        public IActionResult Remove([FromForm] int Id)
        {
            var Server = _servers.GetById(Id);

            if (Server != null)
            {
                Server.SelectedForRemove = false;
                Server.Removed = true;
                Server.RemoveDateTime = DateTime.Now;

                _servers.Update(Server);
                _servers.Save();

                return new ObjectResult(new { type = "remove", text = "Server removed" });
            }

            return new ObjectResult(new { type = "remove", text = "Server not found" });
        }

        public IActionResult MarkForRemove([FromForm] int Id)
        {
            var Server = _servers.GetById(Id);

            if (Server != null)
            {
                Server.SelectedForRemove = true;
                Server.Removed = false;

                _servers.Update(Server);
                _servers.Save();

                return new ObjectResult(new { type = "mark", text = "Server marked" });
            }

            return new ObjectResult(new { type = "mark", text = "Server not found" });
        }

        public IActionResult Add()
        {
            _servers.Add(new Domain.Models.VirtualServer
            {
                CreateDateTime = DateTime.Now,
                SelectedForRemove = false,
                Removed = false
            });

            _servers.Save();

            return new ObjectResult(new { type = "add", text = "Server added" });
        }
    }
}
