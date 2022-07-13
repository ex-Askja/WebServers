namespace WebServers.Domain.Models
{
    public class VirtualServerView
    {
        public int VirtualServerId { get; set; }

        public string? CreateDateTime { get; set; }

        public string? RemoveDateTime { get; set; }

        public bool SelectedForRemove { get; set; }
        public bool Removed { get; set; }
    }
}
