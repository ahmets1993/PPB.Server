using PPB.BL.Interfaces;
using PPB.DAL;
using PPB.DAL.Concrete;

namespace PPB.BL.Repositories
{

    public class UsersRepository:EntityFrameworkRepository<Users>,IUsers
    {
        private DatabaseContext _context;
        public UsersRepository(DatabaseContext db): base(db)
        {
            _context = db;
        }
    }
}
