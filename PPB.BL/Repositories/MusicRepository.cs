using PPB.BL.Interfaces;
using PPB.DAL;
using PPB.DAL.Concrete;

namespace PPB.BL.Repositories
{

    public class MusicRepository:EntityFrameworkRepository<Musics>,IMusic
    {
        private DatabaseContext _context;
        public MusicRepository(DatabaseContext db): base(db)
        {
            _context = db;
        }
    }
}
