using PPB.BL.Interfaces;
using PPB.DAL;
using PPB.DAL.Concrete;

namespace PPB.BL.Repositories
{

    public class PlayerLobbyRepository : EntityFrameworkRepository<PlayerLobby>, IPlayerLobby
    {
        private DatabaseContext _context;
        public PlayerLobbyRepository(DatabaseContext db) : base(db)
        {
            _context = db;
        }
    }
}
