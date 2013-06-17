using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2NET_Dal.Model;

namespace _2NET_Dal
{
    public class Project2NetContext : DbContext
    {
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemsTypess { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Weapon> Weapons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Weapon>()
                        .HasRequired(p => p.Player)
                        .WithMany(w => w.WeaponInventory);

            modelBuilder.Entity<Item>()
                        .HasRequired(p => p.Player)
                        .WithMany(i => i.ObjectInventory);
        }
    }
}
