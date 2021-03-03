using PPB.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPB.BL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUsers Users { get; }
        IPlayerLobby PlayerLobby { get; }
        IBattleLogs BattleLogs { get; }
        IMusic Musics { get; }
        IUserMusics UserMusics { get; }

        object Entity<T>();
        int Save();
    }
}
