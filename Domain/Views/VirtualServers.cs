using WebServers.Domain.Models;

namespace WebServers.Domain.Views
{
    public class VirtualServers
    {
        public List<VirtualServerView>? Servers { get; set; }

        public string? TotalUsageTime { get; set; }

        public string? CurrentDateTime { get; set; }
    }
}
