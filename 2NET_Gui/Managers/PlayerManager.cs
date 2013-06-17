using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _2NET_Dal;
using _2NET_Dal.Model;

namespace _2NET_Gui.Managers
{
    public class PlayerManager
    {
        public Player Player { get; set; }
    
        //New Player
        public PlayerManager(string name)
        {
            if (name != "")
            {
                var db = new Project2NetContext();
                var rand = MainWindow.Random;
                //Randomisation du point de départ
                var x = rand.Next(-500, 500);
                var y = rand.Next(-500, 500);
                var temp = db.Cells.FirstOrDefault(cell => cell.PosX == x && cell.PosY == y);

                if (temp != null)
                {
                    Player = new Player
                        {
                            Name = name, 
                            MaxHp = 500, 
                            Hp = 500, 
                            Xp = 0, 
                            CurrentCellId = temp.Id
                        };
                }
                else
                {
                    var cellM = new CellManager(x, y);
                    Player = new Player
                        {
                            Name = name,
                            MaxHp = 500,
                            Hp = 500,
                            Xp = 0,
                            CurrentCellId = cellM.Cell.Id
                        };
                }
                var weaponM = new WeaponManager(0, true);

                Player.WeaponInventory = new Collection<Weapon>
                    {
                        weaponM.Weapon
                    };
                Player.ObjectInventory = new Collection<Item>();

                db.Players.Add(Player);
                db.SaveChanges();
            }
            else
            {
                Player = null;
            }
        }

        //Existing Player
        public PlayerManager(int id)
        {
            var db = new Project2NetContext();
            Player = db.Players.SingleOrDefault(player => player.Id == id);
        }

        //Save player to db
        public void Save()
        {
            using (var db = new Project2NetContext())
            {
                var player = (from p in db.Players
                              where p.Id == Player.Id
                              select p).FirstOrDefault();
                if (player != null)
                {
                    player.Hp = Player.Hp;
                    player.MaxHp = Player.MaxHp;
                    player.Xp = Player.Xp;
                    player.CurrentCellId = Player.CurrentCellId;
                }

                db.SaveChanges();
            }
        }

        public bool Delete()
        {
            using (var db = new Project2NetContext())
            {
                var player = (from p in db.Players
                              where p.Id == Player.Id
                              select p).FirstOrDefault();
                if (player != null)
                {
                    db.Players.Remove(player);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
