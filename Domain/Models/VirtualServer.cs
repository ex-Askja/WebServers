using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServers.Domain.Models
{
    public class VirtualServer
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int VirtualServerId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime RemoveDateTime { get; set; }

        public bool SelectedForRemove { get; set; }
        public bool Removed { get; set; }
    }
}
