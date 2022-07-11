using WebServers.Domain.Models;

namespace WebServers.Domain.Interfaces
{
    public interface IServerList
    {
        void Add(VirtualServer server);
        List<VirtualServer> GetAll();
        VirtualServer GetById(int Id);
        void Remove(VirtualServer server);
        void Save();
        void Update(VirtualServer server);
    }
}
