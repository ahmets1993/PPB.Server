using PPB.BL.Interfaces;
using PPB.DAL;
using PPB.DAL.Concrete;

namespace PPB.BL.Repositories
{

    public class BattleLogsRepository : EntityFrameworkRepository<BattleLogs>, IBattleLogs
    {
        private DatabaseContext _context;
        public BattleLogsRepository(DatabaseContext db): base(db)
        {
            _context = db;
        }
    }
}
