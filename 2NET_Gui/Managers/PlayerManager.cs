using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Gui.Extensions;

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

        public Weapon AddWeapon()
        {
            var db = new Project2NetContext();
            var player = (from p in db.Players
                          where p.Id == Player.Id
                          select p).FirstOrDefault();
            var weaponM = new WeaponManager(Player.Xp);
            if (player != null)
            {
                player.Hp = Player.Hp;
                player.MaxHp = Player.MaxHp;
                player.Xp = Player.Xp;
                player.CurrentCellId = Player.CurrentCellId;
                player.WeaponInventory.Add(weaponM.Weapon);
                //Copie en local
                Player.WeaponInventory = player.WeaponInventory;
            }
            db.SaveChanges();
            return weaponM.Weapon;
        }

        public Weapon DropWeapon(int id)
        {
            using (var db = new Project2NetContext())
            {
                var player = (from p in db.Players
                              where p.Id == Player.Id
                              select p).FirstOrDefault();

                if (player == null) return null;
                var weaponM = new WeaponManager((from w in player.WeaponInventory
                                                 where w.Id == id
                                                 select w).FirstOrDefault());
                player.Hp = Player.Hp;
                player.MaxHp = Player.MaxHp;
                player.Xp = Player.Xp;
                player.CurrentCellId = Player.CurrentCellId;
                player.WeaponInventory.Remove(weaponM.Weapon);
                var weapon = (from w in db.Weapons
                              where w.Id == weaponM.Weapon.Id
                              select w).FirstOrDefault();
                db.Weapons.Remove(weapon);
                db.SaveChanges();
                return weapon;
            }
        }

        public Item AddItem(int type = -1)
        {
            var db = new Project2NetContext();
            var player = (from p in db.Players
                          where p.Id == Player.Id
                          select p).FirstOrDefault();
            var itemM = new ItemManager((int)Math.Floor((double)Player.Xp / 100), type);
            itemM.Item = new Item { ObjectType = db.ItemsTypess.Find(itemM.Item.ObjectType.Id) };
            if (player != null)
            {
                player.Hp = Player.Hp;
                player.MaxHp = Player.MaxHp;
                player.Xp = Player.Xp;
                player.CurrentCellId = Player.CurrentCellId;
                player.ObjectInventory.Add(itemM.Item);
                Player.ObjectInventory = player.ObjectInventory;
            }
            db.SaveChanges();
            return itemM.Item;
        }

        public Item DropItem(int id)
        {
            using (var db = new Project2NetContext())
            {
                var player = (from p in db.Players
                              where p.Id == Player.Id
                              select p).FirstOrDefault();

                if (player == null) return null;
                var itemM = new ItemManager((from i in player.ObjectInventory
                                                 where i.Id == id
                                                 select i).FirstOrDefault());
                player.Hp = Player.Hp;
                player.MaxHp = Player.MaxHp;
                player.Xp = Player.Xp;
                player.CurrentCellId = Player.CurrentCellId;
                player.ObjectInventory.Remove(itemM.Item);
                var item = (from w in db.Items
                              where w.Id == itemM.Item.Id
                              select w).FirstOrDefault();
                db.Items.Remove(item);
                db.SaveChanges();
                return item;
            }
        }

        public String UseItem(Item item)
        {
            if (item.ObjectType.Type != 0) return "Vous ne pouvez utiliser que des potions de soins hors combat !";
            //Soin
            var hp = Player.Hp;
            Player.Hp += item.ObjectType.HpRestoreValue;
            if (Player.Hp > Player.MaxHp) Player.Hp = Player.MaxHp;
            hp = Player.Hp - hp;

            //Suppression
            using (var db = new Project2NetContext())
            {
                var player = (from p in db.Players
                              where p.Id == Player.Id
                              select p).FirstOrDefault();
                string itemName = "";
                if (player != null)
                {
                    player.Hp = Player.Hp;
                    player.MaxHp = Player.MaxHp;
                    player.Xp = Player.Xp;
                    player.CurrentCellId = Player.CurrentCellId;
                    player.ObjectInventory.Remove(item);
                    //Copie en local
                    Player.ObjectInventory = player.ObjectInventory;

                    itemName = item.ObjectType.Name;
                    item = (from i in db.Items
                            where i.Id == item.Id
                            select i).FirstOrDefault();
                    db.Items.Remove(item);
                }

                db.SaveChanges();
                return string.Format("L'item {0} vous à soigné pour {1} Hp !", itemName, hp);
            }
        }
        public ObservableCollection<Item> GetItems()
        {
            return (from i in Player.ObjectInventory select i).ToObservableCollection();
        }

        public ObservableCollection<Weapon> GetWeapons()
        {
            return (from w in Player.WeaponInventory select w).ToObservableCollection();
        }

        public String GetFormatedLife
        {
            
            get
            {
                if (Player.Hp <= 0)
                {
                    return "0 / " + Player.MaxHp;
                }
                else
                {
                    return Player.Hp + " / " + Player.MaxHp; 
                }
            }
        }

        public String GetFormatedXp
        {
            get { return (Math.Floor((double)Player.Xp / 100)) + " (" + Player.Xp + " Exp)"; }
        }

        public object GetLevel
        {
            get { return "Lvl " + (Math.Floor((double) Player.Xp/100)); }
        }
    }
}
