using Microsoft.EntityFrameworkCore;
using PPB.DAL.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPB.DAL
{
    public class DatabaseContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
     => options.UseNpgsql(
         "Server=localhost;Port=5432;User Id=postgres;Password=secret;Database=PPB;",
         x => x.MigrationsHistoryTable("__EFMigrationsHistory", "PPB"));

        public DbSet<Users> Users { get; set; }
        public DbSet<PlayerLobby> PlayerLobby { get; set; }
        public DbSet<BattleLogs> BattleLogs { get; set; }

        public DbSet<Musics> Musics { get; set; }
        public DbSet<UserMusics> UserMusics { get; set; }

    }
}
