using WebServers.Domain.Interfaces;
using WebServers.Domain.Models;

namespace WebServers.Domain.Data
{
    public class ServerList : IServerList
    {
        private AppDbContext _context { get; set; }

        public ServerList(AppDbContext context)
        {
            this._context = context;
        }

        public List<VirtualServer> GetAll()
        {
            return this._context.VirtualServers.ToList();
        }

        public VirtualServer GetById(int Id)
        {
            return this._context.VirtualServers.Where(s => s.VirtualServerId.Equals(Id)).FirstOrDefault();
        }

        public void Add(VirtualServer server)
        {
            this._context.Add(server);
        }

        public void Update(VirtualServer server)
        {
            this._context.Update(server);
        }

        public void Remove(VirtualServer server)
        {
            this._context.Remove(server);
        }

        public void Save()
        {
            this._context.SaveChanges();
        }
    }
}
