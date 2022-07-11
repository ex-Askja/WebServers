using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebServers.Domain.Interfaces;
using WebServers.Domain.Models;
using WebServers.Domain.Views;
using WebServers.Models;

namespace WebServers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServerList _servers;

        public HomeController(ILogger<HomeController> logger, IServerList servers)
        {
            _logger = logger;
            _servers = servers;
        }

        public IActionResult Index()
        {
            var Now = DateTime.Now;

            VirtualServerView Model = new()
            {
                CurrentDateTime = Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Servers = _servers.GetAll() ?? new List<VirtualServer>()
            };

            double TotalTime = 0;

            foreach (var Server in Model.Servers)
            {
                if (Server.Removed) {
                    TotalTime += (Server.RemoveDateTime - Server.CreateDateTime).TotalSeconds;
                } else {
                    TotalTime += (Now - Server.CreateDateTime).TotalSeconds;
                }
            }

            Model.TotalUsageTime = new DateTime(TimeSpan.FromSeconds(TotalTime).Ticks).ToString("HH:mm:ss");

            return View(Model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}