using PPB.BL.Interfaces;
using PPB.DAL;
using PPB.DAL.Concrete;

namespace PPB.BL.Repositories
{

    public class UserMusicsRepository : EntityFrameworkRepository<UserMusics>, IUserMusics
    {
        private DatabaseContext _context;
        public UserMusicsRepository(DatabaseContext db) : base(db)
        {
            _context = db;
        }
    }
}
