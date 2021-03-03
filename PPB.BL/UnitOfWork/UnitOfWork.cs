using PPB.BL.Interfaces;
using PPB.BL.Repositories;
using PPB.DAL;
using System;

namespace PPB.BL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUsers Users {get;}
        public IPlayerLobby PlayerLobby { get; } 
        public IBattleLogs BattleLogs { get; }
        public IMusic Musics { get; }
        public IUserMusics UserMusics { get; }

        private readonly DatabaseContext _context;
        public UnitOfWork()
        {
            _context = new DatabaseContext();
            Users = new UsersRepository(_context);
            PlayerLobby = new PlayerLobbyRepository(_context);
            BattleLogs = new BattleLogsRepository(_context);
            Musics = new MusicRepository(_context);
            UserMusics = new UserMusicsRepository(_context);


        }

        public void Dispose()
        {

            _context.Dispose();

        }
        public int Save()
        {
            return  _context.SaveChanges(); 
        }

        public object Entity<T>()
        {
            throw new NotImplementedException();
        }
    }
}
