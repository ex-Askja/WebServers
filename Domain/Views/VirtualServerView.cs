using WebServers.Domain.Models;

namespace WebServers.Domain.Views
{
    public class VirtualServerView
    {
        public List<VirtualServer>? Servers { get; set; }

        public string? TotalUsageTime { get; set; }

        public string? CurrentDateTime { get; set; }
    }
}
