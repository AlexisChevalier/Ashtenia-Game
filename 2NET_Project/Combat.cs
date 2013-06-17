using System;
using System.Collections.Generic;
using System.Linq;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Project.Managers;

namespace _2NET_Project
{
    internal class Combat
    {

        //Managers
        public MonsterManager MonsterM { get; set; }
        public PlayerManager PlayerM { get; set; }

        //Attack Rates
        public int PlayerAttackRate { get; set; }
        public int MonsterAttackRate { get; set; }

        //Temporary Bonuses
        public int PlayerBonusAttaque { get; set; }
        public int PlayerBonusDefense { get; set; }
        public int MonsterBonusAttaque { get; set; }
        public int MonsterBonusDefense { get; set; }

        //Constructeur
        public Combat(int idPlayer, Cell cell)
        {
            PlayerM = new PlayerManager(idPlayer);
            MonsterM = new MonsterManager(PlayerM.Player, cell);
            MonsterAttackRate = MonsterM.Monster.AttackRate;
            PlayerAttackRate = 0;

            PlayerBonusAttaque = 0;
            PlayerBonusDefense = 0;
            MonsterBonusAttaque = 0;
            MonsterBonusDefense = 0;
        }

        //Démarrage du combat
        public Player Start()
        {
            ShowIntro();
            while (true)
            {
                if (PlayerTurn()) //Si gagné
                {
                    PlayerWon();
                    break;
                }

                if (MonsterTurn()) //Si perdu
                {
                    PlayerLost();
                    break;
                }
            }
            return PlayerM.Player;
        }

        //Tour du monstre
        public bool MonsterTurn() // Si pas de tué, retourne false
        {
            Console.Clear();
            var damage = AttackPlayer();
            ShowCombatInfoLine(1);
            if (damage != -1)
            {
                if (PlayerM.Player.Hp <= 0)
                {
                    Console.WriteLine("{0} vous frappe pour {1} dégats, vous mourrez.", MonsterM.Monster.Name,
                                            damage);
                }
                else
                {
                    Console.WriteLine("{0} vous frappe pour {1} dégats, il vous reste {2} PVs.", MonsterM.Monster.Name,
                                            damage, PlayerM.Player.Hp);
                }
                
            }
            else
            {
                Console.WriteLine("{0} à raté son coup !", MonsterM.Monster.Name);
            }

            //Est-il assez rapide ?
            if (PlayerAttackRate > MonsterAttackRate && PlayerM.Player.Hp > 0)
            {
                var rand = Program.Random;
                if (rand.Next(100) > 60)
                {//Oui, on peut frapper une deuxiéme fois
                    Console.WriteLine();
                    Console.WriteLine("Vous avez été suffisement rapide pour donner un second coup !");
                    Console.WriteLine();
                    Console.WriteLine("Appuyez sur entrée pour continuer.");
                    Console.ReadLine();
                    Console.Clear();
                    damage = AttackPlayer();
                    //Affichage du résultat
                    ShowCombatInfoLine(0);
                    if (damage != -1)
                    {
                        if (PlayerM.Player.Hp <= 0)
                        {
                            Console.WriteLine("{0} vous frappe pour {1} dégats, vous mourrez.", MonsterM.Monster.Name,
                                                    damage);
                        }
                        else
                        {
                            Console.WriteLine("{0} vous frappe pour {1} dégats, il vous reste {2} PVs.", MonsterM.Monster.Name,
                                                    damage, PlayerM.Player.Hp);
                        }

                    }
                    else
                    {
                        Console.WriteLine("{0} à raté son coup !", MonsterM.Monster.Name);
                    }
                }
            }


            Console.WriteLine();
            Console.WriteLine("Appuyez sur entrée pour continuer.");
            Console.ReadLine();
            return PlayerM.Player.Hp <= 0;
        }

        //Tour du joueur
        public bool PlayerTurn() // Si pas de tué, retourne false
        {
            do
            {
                Console.Clear();
            } while (!ShowPlayerOptions());

            return MonsterM.Monster.Hp <= 0;
        }

        //Affiche les options du joueur
        public bool ShowPlayerOptions()
        {
            bool result;
            do
            {
                Console.Clear();
                ShowCombatInfoLine(0);
                Console.WriteLine("Que désirez vous faire ?");
                Console.WriteLine("1- Attaquer avec une arme");
                Console.WriteLine("2- Utiliser un objet");
                var temp = Console.ReadLine();
                switch (temp)
                {
                    case "1":
                        result = WeaponAction();
                        break;
                    case "2":
                        result = ObjectAction();
                        break;
                    default:
                        return false;
                }
            } while (result == false);

            return true;
        }

        //Affiche les actions liées au armes -- Effectue le second coup si besoin
        public bool WeaponAction()
        {
            Weapon selectedWeapon;
            do
            {
                string weaponId;
                int id;
                do
                {
                    Console.Clear();
                    ShowCombatInfoLine(0);
                    Console.WriteLine("Saisissez l'ID de l'arme que vous voulez utiliser (esc pour annuler) : ");
                    Console.WriteLine();
                    PlayerM.ShowWeapons();
                    Console.WriteLine();
                    weaponId = Console.ReadLine();
                    if (weaponId == "esc") return false;
                } while (!int.TryParse(weaponId, out id));

                selectedWeapon =
                    PlayerM.Player.WeaponInventory.SingleOrDefault(weapon => weapon.Id == id);
            } while (selectedWeapon == null);
            PlayerAttackRate = selectedWeapon.AttackRate;
            var damage = AttackMonster(selectedWeapon);
            Console.Clear();
            //Affichage du résultat
            ShowCombatInfoLine(0);
            if (damage != -1)
            {
                if (MonsterM.Monster.Hp <= 0)
                {
                    Console.WriteLine("Vous frappez {0} pour {1} dégats, il meurt", MonsterM.Monster.Name,
                                                    damage);
                }
                else
                {
                    Console.WriteLine("Vous frappez {0} pour {1} dégats, il lui reste {2} PVs.",
                                                    MonsterM.Monster.Name, damage, MonsterM.Monster.Hp);
                }
            }
            else
            {
                Console.WriteLine("Vous avez raté votre coup !");
            }
            
            //Est-il assez rapide ?
            if (MonsterAttackRate > PlayerAttackRate && MonsterM.Monster.Hp >= 0)
            {
                var rand = Program.Random;
                if (rand.Next(100) > 60)
                {//Oui, on peut frapper une deuxiéme fois avec la même arme
                    Console.WriteLine();
                    Console.WriteLine("Vous avez été suffisement rapide pour donner un second coup !");
                    Console.WriteLine();
                    Console.WriteLine("Appuyez sur entrée pour continuer.");
                    Console.ReadLine();
                    Console.Clear();
                    damage = AttackMonster(selectedWeapon);
                    //Affichage du résultat
                    ShowCombatInfoLine(0);
                    if (damage != -1)
                    {
                        if (MonsterM.Monster.Hp <= 0)
                        {
                            Console.WriteLine("Vous frappez {0} pour {1} dégats, il meurt", MonsterM.Monster.Name,
                                                            damage);
                        }
                        else
                        {
                            Console.WriteLine("Vous frappez {0} pour {1} dégats, il lui reste {2} PVs.",
                                                            MonsterM.Monster.Name, damage, MonsterM.Monster.Hp);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Vous avez raté votre coup !");
                    }
                }
            }
                Console.WriteLine();
                Console.WriteLine("Appuyez sur entrée pour continuer.");
                Console.ReadLine();
            
            return true;
        }

        //Affiche les actions liées aux objets
        public bool ObjectAction()
        {
            Item selectedItem;
            do
            {
                string objetId;
                int id;
                do
                {
                    Console.Clear();
                    ShowCombatInfoLine(0);
                    Console.WriteLine("Saisissez l'ID de l'objet que vous voulez utiliser (esc pour annuler) : ");
                    Console.WriteLine();
                    PlayerM.ShowInventory();
                    Console.WriteLine();
                    objetId = Console.ReadLine();
                    if (objetId == "esc") return false;
                } while (!int.TryParse(objetId, out id));
                selectedItem = PlayerM.Player.ObjectInventory.SingleOrDefault(objet => objet.Id == id);
            } while (selectedItem == null);

            if (selectedItem.ObjectType.Type == 0) //Health Potion
            {
                //Soin
                var hp = PlayerM.Player.Hp;
                PlayerM.Player.Hp += selectedItem.ObjectType.HpRestoreValue;
                if (PlayerM.Player.Hp > PlayerM.Player.MaxHp) PlayerM.Player.Hp = PlayerM.Player.MaxHp;
                hp = PlayerM.Player.Hp - hp;
                Console.WriteLine("L'item {0} vous à soigné pour {1} Hp !", selectedItem.ObjectType.Name, hp );
            }
            else
            {
                if (selectedItem.ObjectType.AttackStrenghtBonus != 0)
                {
                    Console.WriteLine("L'item {0} vous à rendu plus fort de {1} Points !", selectedItem.ObjectType.Name, selectedItem.ObjectType.AttackStrenghtBonus);
                    PlayerBonusAttaque += selectedItem.ObjectType.AttackStrenghtBonus;
                }

                if (selectedItem.ObjectType.DefenseBoost != 0)
                {
                    Console.WriteLine("L'item {0} vous à rendu plus résistant de {1} Points !", selectedItem.ObjectType.Name, selectedItem.ObjectType.DefenseBoost);
                    PlayerBonusDefense += selectedItem.ObjectType.DefenseBoost;
                }
            }

            //Suppression
            var db = new Project2NetContext();
            var player = (from p in db.Players
                            where p.Id == PlayerM.Player.Id
                            select p).FirstOrDefault();
            if (player != null)
            {
                player.Hp = PlayerM.Player.Hp;
                player.ObjectInventory.Remove(selectedItem);
                //Copie en local
                PlayerM.Player.ObjectInventory = player.ObjectInventory;

                var item = (from i in db.Items
                            where i.Id == selectedItem.Id
                            select i).FirstOrDefault();
                db.Items.Remove(item);
            }

            Console.WriteLine();
            Console.WriteLine("Appuyez sur entrée pour continuer");
            Console.ReadLine();
            Console.Clear();

            db.SaveChanges();
            return true;
        }

        //Envoie une attaque sur le joueur
        public int AttackPlayer()
        {
            var rand = Program.Random;
            var missChances = rand.Next(0, 101);
            int damage;
            if (missChances > (100 - MonsterM.Monster.MissRate))
            {
                damage = -1;
            }
            else
            {
                damage = rand.Next(MonsterM.Monster.Damage - 5, MonsterM.Monster.Damage + 6);
                damage = damage + MonsterBonusAttaque - PlayerBonusDefense;
                if (damage <= 0) damage = 0;
                PlayerM.Player.Hp -= damage;
                PlayerM.Save();
            }
            
            return damage;
        }

        //Envoie une attaque sur le monstre
        public int AttackMonster(Weapon arme)
        {
            var rand = Program.Random;
            var missChances = rand.Next(0, 101);
            int damage;
            if (missChances > (100 - arme.MissRate))
            {
                damage = -1;
            }
            else
            {
                damage = rand.Next(arme.Damage - 5, arme.Damage + 6);
                damage = damage + PlayerBonusAttaque - MonsterBonusDefense;
                if (damage <= 0) damage = 0;
                MonsterM.Monster.Hp -= damage;
            }
            return damage;
        }

        //Effectué si gagné
        public void PlayerWon()
        {
            Console.Clear();
            var rand = Program.Random;
            var xp = rand.Next(20, 50);
            var oldLevel = (int) Math.Floor((double) PlayerM.Player.Xp/100);
            PlayerM.Player.Xp += xp;
            var newLevel = (int) Math.Floor((double) PlayerM.Player.Xp/100);

            if (oldLevel < newLevel)
            {
                PlayerM.Player.MaxHp += 150;
                PlayerM.Player.Hp += 150;
            }

            ShowCombatInfoLine(-1);
            Console.WriteLine("Vous avez gagné le combat ! Votre experience augmente de {0} points !", xp);
            Console.WriteLine();
            //Génération des objets gagnés (70% 1 item, 20% un deuxiéme)
            if (rand.Next(0, 101) > 30)
            {
                var itemM = new ItemManager((int)Math.Floor((double)PlayerM.Player.Xp / 100), PlayerM);
                PlayerM = itemM.PlayerM;
                Console.WriteLine("Vous avez gagné un objet : {0} - Niveau {1}", itemM.Name, itemM.Level);
                if (rand.Next(0, 101) > 80)
                {
                    Console.WriteLine();
                    var secondItemM = new ItemManager((int)Math.Floor((double)PlayerM.Player.Xp / 100), PlayerM);
                    PlayerM = secondItemM.PlayerM;
                    Console.WriteLine("Vous avez gagné un autre objet : {0} - Niveau {1}", secondItemM.Name, secondItemM.Level);
                }
                Console.WriteLine();
            }

            //40% de chances de drop une arme
            if (rand.Next(0, 101) > 60)
            {
                var weaponAdded = PlayerM.AddWeapon();
                Console.WriteLine("Vous avez gagné une arme : {0} - Niveau {1}", weaponAdded.Name, weaponAdded.Level);
                Console.WriteLine();
            }


            if (oldLevel < newLevel)
            {
                Console.WriteLine("Vous êtes passés au niveau {0}, Félicitations !", newLevel);
                Console.WriteLine("Votre vie maximale est maintenant passée a {0} et vous avez été soigné de 150 PVs !", PlayerM.Player.MaxHp);
            }
            
            PlayerM.Save();
            Console.WriteLine();
            Console.WriteLine("Appuyez sur entrée pour continuer");
            Console.ReadLine();
            Console.Clear();
        }

        //Effectué si perdu
        public void PlayerLost()
        {
            Console.Clear();
            Console.WriteLine("                                                                               ");
            Console.WriteLine("        IT'S NOT A LIE                                                         ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                               █                YOU LOOSE (noob) !             ");
            Console.WriteLine("                               █                                               ");
            Console.WriteLine("                                █                                              ");
            Console.WriteLine("                                 █                                             ");
            Console.WriteLine("                            █ ████ ██           █████████                      ");
            Console.WriteLine("                           █ █████ ███ ██████           ██                     ");
            Console.WriteLine("                      █████ █ ███████ █             ██  ██                     ");
            Console.WriteLine("                      ██     █ █████    █       ██  ██████                     ");
            Console.WriteLine("                      █  ██     █     █     ██  ██████████                     ");
            Console.WriteLine("                      █      ██   █     ██  ██████████  ██                     ");
            Console.WriteLine("                      █           ████  ██████████  ██████                     ");
            Console.WriteLine("                      █              █████████  ██████  ██                     ");
            Console.WriteLine("                      █              █████  ██████  ██████                     ");
            Console.WriteLine("                      █              █  ██████  ██████████                     ");
            Console.WriteLine("                      █              █████  ██████████                         ");
            Console.WriteLine("                         ██          █  ██████████                             ");
            Console.WriteLine("                             ██      █████████                                 ");
            Console.WriteLine("                                 ███ █████                                     ");
            Console.WriteLine("                                    ██                                         ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                     APPUYEZ SUR ENTREE POUR CONTINUER                         ");

            Console.ReadLine();
            Console.Clear();

            string response;
            while (true)
            {
                Console.WriteLine("Vous avez décédé.");
                Console.WriteLine("Dans un extrême élan de bonté, le gardien des tombeaux vous propose de repartir ");
                Console.WriteLine("vers le monde afin de poursuivre votre glorieuse quête.");
                Console.WriteLine();
                Console.WriteLine("Mais ceci a un prix, il vous propose donc deux choix :");
                Console.WriteLine("1 - Vous gardez votre personnage, mais perdez 3 niveaux et tous ses objets/armes");
                Console.WriteLine("2 - Vous supprimez ce personnage");
                response = Console.ReadLine();

                var db = new Project2NetContext();
                var player = (from p in db.Players
                              where p.Id == PlayerM.Player.Id
                              select p).FirstOrDefault();

                if (response == "1")
                {
                    if (player != null)
                    {
                        player.Xp = player.Xp - 300;
                        if (player.Xp <= 0) player.Xp = 0;
                        player.MaxHp = 500 + ((int) Math.Floor((double) player.Xp/100))*150;
                        player.Hp = player.MaxHp;
                        var itemList = player.ObjectInventory.ToList();
                        foreach (var item in itemList)
                        {
                            player.ObjectInventory.Remove(item);
                            var itemD = (from i in db.Items
                                         where i.Id == item.Id
                                         select i).FirstOrDefault();
                            db.Items.Remove(itemD);
                        }

                        var weaponList = player.WeaponInventory.ToList();
                        foreach (var weapon in weaponList)
                        {
                            player.WeaponInventory.Remove(weapon);
                            var weaponD = (from i in db.Weapons
                                           where i.Id == weapon.Id
                                           select i).FirstOrDefault();
                            db.Weapons.Remove(weaponD);
                        }

                        var weaponM = new WeaponManager(player.Xp, true);
                        player.WeaponInventory.Add(weaponM.Weapon);
                        db.SaveChanges();
                        PlayerM.Player = player;
                        Console.Clear();
                        Console.WriteLine("Vous vous sentez renaître !");
                        Console.WriteLine();
                        Console.WriteLine("Appuyez sur une touche pour continuer.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                }
                else if (response == "2")
                {
                    db.Players.Remove(player);
                    db.SaveChanges();
                    Console.WriteLine("*Pouf*, Le personnage à été détruit ! Appuyez sur une touche pour quitter.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }


        //Affiche la super intro du combat (amazing)
        public void ShowIntro()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Vous rencontrez une créature hostile, Appuyez sur Entrée pour combattre !");
            Console.ResetColor();
            Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████    ████████     █████████     ████████     █      █     █████████     ████");
            Console.WriteLine("████    █                █         █            █      █         █         ████");
            Console.WriteLine("████    █                █         █            █      █         █         ████");
            Console.WriteLine("████    ██████           █         █   ████     ████████         █         ████");
            Console.WriteLine("████    █                █         █   █  █     █      █         █         ████");
            Console.WriteLine("████    █                █         █      █     █      █         █         ████");
            Console.WriteLine("████    █            █████████     ████████     █      █         █         ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.Write("████     ");
            Console.Write(PlayerM.Player.Name);
            for (int i = 0; i < (66 - PlayerM.Player.Name.Length); i++)
            {
                Console.Write(" ");
            }
            Console.Write("████");
            Console.WriteLine();
            Console.WriteLine("████      VS                                                               ████");
            Console.Write("████     ");
            Console.Write(MonsterM.Monster.Name);
            for (int i = 0; i < (66 - MonsterM.Monster.Name.Length); i++)
            {
                Console.Write(" ");
            }
            Console.Write("████");
            Console.WriteLine();
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                  Appuyez sur un bouton pour commencer                 ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        //Permet d'afficher les barres d'info en haut
        public void ShowCombatInfoLine(int turn) //0 pour le joueur, 1 pour le monstre
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            if (turn == 0)
            {
                Console.WriteLine("                        COMBAT EN COURS - TOUR DU JOUEUR                       ");
            }
            else if (turn == 1)
            {
                Console.WriteLine("                      COMBAT EN COURS - TOUR DE LA CREATURE                    ");
            }
            else
            {
                Console.WriteLine("                                 COMBAT EN COURS                               ");
            }

            Console.ResetColor();

            //Affichage de l'ennemi

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("   Créature : ");

            //Nom
            Console.Write(MonsterM.Monster.Name);
            for (var i = 0; i < (40 - MonsterM.Monster.Name.Length); i++)
            {
                Console.Write(" ");
            }
            //Life
            var life = MonsterM.Monster.Hp;
            if (life <= 0) life = 0;
            var stringifiedLife = life.ToString();
            Console.Write("             Vie : ");
            Console.Write(stringifiedLife);
            for (var i = 0; i < (6 - stringifiedLife.Length); i++)
            {
                Console.Write(" ");
            }

            Console.ResetColor();
            Console.WriteLine();


            //Affichage du joueur

            PlayerM.ShowPlayerInfos();
        }
    }
}