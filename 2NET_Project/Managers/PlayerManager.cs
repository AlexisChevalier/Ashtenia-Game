using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _2NET_Dal;
using _2NET_Dal.Model;

namespace _2NET_Project.Managers
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
                var rand = Program.Random;
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

        //Add item then save it

        public void AddItem(int item)
        {
            var db = new Project2NetContext();
                var player = (from p in db.Players
                              where p.Id == Player.Id
                              select p).FirstOrDefault();
                if (player != null)
                {
                    player.Hp = Player.Hp;
                    player.MaxHp = Player.MaxHp;
                    player.Xp = Player.Xp;
                    player.CurrentCellId = Player.CurrentCellId;
                    player.ObjectInventory.Add(new Item {ObjectType = db.ItemsTypess.Find(item)});
                    //Copie en local
                    Player.ObjectInventory = player.ObjectInventory;
                }
                db.SaveChanges();
        }

        //Add Weapon then save it

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

        //Drop Weapon
        public bool DropWeapon()
        {
            if (Player.WeaponInventory.Count <= 1)
            {
                Console.WriteLine("Vous devez garder au moins une arme !");
                return false;
            }
            Weapon weapon;
            do
            {
                string stringId;
                int id;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Saisissez l'ID de l'arme que vous voulez jeter (esc pour quitter)");
                    ShowWeapons();
                    stringId = Console.ReadLine();
                    if (stringId == "esc")
                    {
                        Console.Clear();
                        ShowPlayerInfos();
                        return false;
                    }
                } while (!int.TryParse(stringId, out id));
                weapon = Player.WeaponInventory.SingleOrDefault(w => w.Id == id);
            } while (weapon == null);

            using (var db = new Project2NetContext())
            {
                var player = (from p in db.Players
                              where p.Id == Player.Id
                              select p).FirstOrDefault();
                var weaponM = new WeaponManager(weapon);
                if (player != null)
                {
                    player.Hp = Player.Hp;
                    player.MaxHp = Player.MaxHp;
                    player.Xp = Player.Xp;
                    player.CurrentCellId = Player.CurrentCellId;
                    player.WeaponInventory.Remove(weaponM.Weapon);
                    //Copie en local
                    Player.WeaponInventory = player.WeaponInventory;
                    weapon = (from w in db.Weapons
                                  where w.Id == weaponM.Weapon.Id
                                  select w).FirstOrDefault();
                    db.Weapons.Remove(weapon);
                }

                db.SaveChanges();
                Console.WriteLine("L'arme {0} à été supprimée", weapon.Name);
                Console.WriteLine();
                Console.WriteLine("Appuyez sur entrée pour continuer");
                Console.ReadLine();
                Console.Clear();
                ShowPlayerInfos();
            }
            return true;
        }

        //Drop Item
        public bool DropItem()
        {
            Item item;
            do
            {
                string stringId;
                int id;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Saisissez l'ID de l'item que vous voulez jeter (esc pour quitter)");
                    ShowInventory();
                    stringId = Console.ReadLine();
                    if (stringId == "esc")
                    {
                        Console.Clear();
                        ShowPlayerInfos();
                        return false;
                    }
                } while (!int.TryParse(stringId, out id));
                 item = Player.ObjectInventory.SingleOrDefault(i => i.Id == id);
            } while (item == null);
           
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
                Console.WriteLine("L'item {0} à été supprimé", itemName);
                Console.WriteLine();
                Console.WriteLine("Appuyez sur entrée pour continuer");
                Console.ReadLine();
                Console.Clear();
                ShowPlayerInfos();
            }
            return true;
        }

        //Use item out of fight
        public bool UseItem()
        {

            Item item;
            do
            {
                string stringId;
                int id;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Saisissez l'ID de l'item que vous voulez utiliser (esc pour quitter)");

                    var usableItems = from p in Player.ObjectInventory where p.ObjectType.Type == 0 select p;
                    if (!usableItems.Any())
                    {
                        Console.WriteLine("Vous n'avez aucun objet utilisable hors combat.");
                        return false;
                    }
                    else
                    {
                        foreach (var usableItem in usableItems)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("ID : {0} NOM : {1} | NIVEAU {2}", usableItem.Id,
                                              usableItem.ObjectType.Name, usableItem.ObjectType.Level);
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.WriteLine("   -> [+HP : {0}]", usableItem.ObjectType.HpRestoreValue);
                            Console.WriteLine();
                            Console.ResetColor();
                        }
                    }

                    stringId = Console.ReadLine();
                    if (stringId == "esc")
                    {
                        Console.Clear();
                        ShowPlayerInfos();
                        return false;
                    }
                } while (!int.TryParse(stringId, out id));
                item = Player.ObjectInventory.SingleOrDefault(i => i.Id == id);
            } while (item == null);

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
                Console.WriteLine("L'item {0} vous à soigné pour {1} Hp !", itemName,hp );
                Console.WriteLine();
                Console.WriteLine("Appuyez sur entrée pour continuer");
                Console.ReadLine();
                Console.Clear();
                ShowPlayerInfos();
            }
            return true;
        }

        //Recherche des items dans la zone
        public bool SearchCellForItems()
        {
            if (GetCellManager().HadBeenVisited())
            {
                Console.WriteLine("Cette zone à déja été fouillée.");
                Console.WriteLine();
            }
            else
            {
                GetCellManager().IsVisited();
                var rand = Program.Random;
                if (rand.Next(0, 101) > 90) //10 % de chances de trouver un item
                {
                    var itemM = new ItemManager((int)Math.Floor((double)Player.Xp / 100), this);
                    Player = itemM.PlayerM.Player;
                    Console.WriteLine("Quelle chance ! Vous avez trouvé un objet !");
                    Console.WriteLine("         -> {0} - Niveau {1}", itemM.Name, itemM.Level);
                }
                else if (rand.Next(0, 101) > 95) //5% de trouver une arme
                {
                    var weaponAdded = AddWeapon();
                    Console.WriteLine("Petit veinard ! Vous avez trouvé une arme !");
                    Console.WriteLine("         -> {0} - Niveau {1}", weaponAdded.Name, weaponAdded.Level);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Vous n'avez rien trouvé !");
                    Console.WriteLine();
                }
            }
            return true;
        }

        //Déplacements
        public bool Move(string direction)
        {
            var cellM = new CellManager((int)Player.CurrentCellId);
            var canMoveTo = cellM.ToArrayCanMoveTo();
            var index = -1;
            switch (direction)
            {
                case "nord":
                    index = 0;
                break;
                case "est":
                    index = 1;
                break;
                case "sud":
                    index = 2;
                break;
                case "ouest":
                    index = 3;
                break;
                default:
                return false;
            }
            if (canMoveTo[index] == '1')
            {
                cellM = new CellManager(cellM.Cell.PosX, cellM.Cell.PosY, index);
                /* Va-t-il y avoir un combat ?*/
                var rand = Program.Random;

                
                if(rand.Next(0, 101) < cellM.Cell.MonsterRate) //YEAAAAAAH CA VA CHAUFFER A DONF 
                {
                    var fight = new Combat(Player.Id, cellM.Cell);
                    var newPlayer = fight.Start();
                    Player = newPlayer;
                    Player.CurrentCellId = cellM.Cell.Id;
                    ShowPlayerInfos();
                    Console.WriteLine(GetCellManager().Cell.Description);
                    Save();

                }
                else
                {
                    Player.CurrentCellId = cellM.Cell.Id;
                    Console.WriteLine(GetCellManager().Cell.Description);
                    Save(); 
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Le chemin est bloqué !");
                return false;
            }
            return true;
        }

        //Show green top bar for infos
        public void ShowPlayerInfos()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write("   ");

            //Nom
            Console.Write(Player.Name);
            for (var i = 0; i < (20 - Player.Name.Length); i++) 
            {
                Console.Write(" ");
            }
            Console.Write("     ");

            //XP
            var xp = ((int)Math.Floor((double)Player.Xp/100)).ToString();
            for (var i = 0; i < (14 - xp.Length); i++) 
            {
                Console.Write(" ");
            }
            Console.Write("Level : ");
            Console.Write(xp);
            Console.Write("      ");

            //Life
            var life = Player.Hp;
            if (life <= 0) life = 0;
            var stringifiedLife = life.ToString();
            for (var i = 0; i < (14 - stringifiedLife.Length); i++) 
            {
                Console.Write(" ");
            }
            Console.Write("Vie : ");
            Console.Write(stringifiedLife);

            Console.Write("   ");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine();
        }

        //Displays weapons
        public void ShowWeapons()
        {
            if (Player.WeaponInventory.Count == 0)
            {
                Console.WriteLine("Vous n'avez aucune arme actuellement.");
            }
            else
            {
                foreach (var arme in Player.WeaponInventory)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("ID : {0} NOM : {1} | NIVEAU {2}", arme.Id, arme.Name, arme.Level);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("   -> [+VITESSE ATTAQUE : {0} | +CHANCES RATER : {1} | DEGATS : {2}]", arme.AttackRate, arme.MissRate, arme.Damage);
                    Console.WriteLine();
                }
                Console.ResetColor();
            }
        }

        //Displays Items
        public void ShowInventory()
        {
            if (Player.ObjectInventory.Count == 0)
            {
                Console.WriteLine("Vous n'avez aucun objet actuellement.");
            }
            else
            {
                foreach (var item in Player.ObjectInventory)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("ID : {0} NOM : {1} | NIVEAU {2}", item.Id, item.ObjectType.Name, item.ObjectType.Level);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("   -> [+ATTAQUE : {0} | +DEFENSE : {1} | +HP : {2}]", item.ObjectType.AttackStrenghtBonus, item.ObjectType.DefenseBoost, item.ObjectType.HpRestoreValue);
                    Console.WriteLine();
                }
                Console.ResetColor();
            }
        }

        //Return CellManager
        public CellManager GetCellManager()
        {
            using (var db = new Project2NetContext())
            {
                return new CellManager(db.Cells.SingleOrDefault(cell => cell.Id == Player.CurrentCellId));
            }
        }
    }
}
