using Microsoft.AspNetCore.Mvc;
using WebServers.Domain.Interfaces;
using WebServers.Domain.Models;
using WebServers.Domain.Views;

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

            if (Server != null && !Server.Removed && Server.SelectedForRemove)
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

            if (Server != null && !Server.Removed && !Server.SelectedForRemove)
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

        public IActionResult Load()
        {
            var Now = DateTime.Now;

            VirtualServers Model = new()
            {
                CurrentDateTime = Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            double TotalTime = 0;
            var AllServers = _servers.GetAll() ?? new List<VirtualServer>();
            var ResultServers = new List<VirtualServerView>();

            foreach (var Server in AllServers)
            {
                if (Server.Removed)
                {
                    TotalTime += (Server.RemoveDateTime - Server.CreateDateTime).TotalSeconds;
                }
                else
                {
                    TotalTime += (Now - Server.CreateDateTime).TotalSeconds;
                }

                ResultServers.Add(new VirtualServerView()
                {
                    VirtualServerId = Server.VirtualServerId,
                    CreateDateTime = Server.CreateDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                    RemoveDateTime = Server.RemoveDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                    Removed = Server.Removed,
                    SelectedForRemove = Server.SelectedForRemove
                });
            }

            Model.Servers = ResultServers;
            Model.TotalUsageTime = new DateTime(TimeSpan.FromSeconds(TotalTime).Ticks).ToString("HH:mm:ss");

            return new ObjectResult(Model);
        }
    }
}
