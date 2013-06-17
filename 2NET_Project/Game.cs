using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Project.Managers;

namespace _2NET_Project
{
    class Game
    {
        //Affiche les commandes dispo
        public void GetAvailableCommands()
        {
            Console.WriteLine("-------------- ASHTENIA GAME AIDE --------------");
            Console.WriteLine("-> Déplacements");
            Console.WriteLine("Aller au nord");
            Console.WriteLine("Aller a l'est");
            Console.WriteLine("Aller au sud");
            Console.WriteLine("Aller a l'ouest"); 
            Console.WriteLine("Fouiller la zone");
            Console.WriteLine("ou suis-je");
            Console.WriteLine();
            Console.WriteLine("-> Inventaire");
            Console.WriteLine("Afficher l'inventaire");
            Console.WriteLine("Utiliser un objet");
            Console.WriteLine("Jeter un objet");
            Console.WriteLine("Afficher les armes");
            Console.WriteLine("Jeter une arme");
            Console.WriteLine();
            Console.WriteLine("-> Menu");
            Console.WriteLine("Aide");
            Console.WriteLine("Quitter");
            Console.WriteLine("Vous pouvez aussi taper les cheat codes ici.");
        }

        //Interpreteur de commandes
        //Permet d'effectuer les actions en fonction de la commande utilisée
        public bool CheckCommand(string command, PlayerManager activePlayerM)
        {
            Console.Clear();
            activePlayerM.ShowPlayerInfos();
            command = command.ToLower();
            var returnVal = true;

            switch (command)
            {
                //Inventaire
                case "afficher l'inventaire":
                    activePlayerM.ShowInventory();
                    break;
                case "jeter un objet":
                    activePlayerM.DropItem();
                    break;
                case "utiliser un objet":
                    activePlayerM.UseItem();
                    break;
                case "afficher les armes":
                    activePlayerM.ShowWeapons();
                    break;
                case "jeter une arme":
                    activePlayerM.DropWeapon();
                    break;
                case "fouiller la zone":
                    activePlayerM.SearchCellForItems();
                    break;

                //Déplacements
                case "aller au nord":
                    Console.WriteLine("Vous vous déplacez vers le nord");
                    Console.WriteLine();
                    activePlayerM.Move("nord");
                    break;
                case "aller au sud":
                    Console.WriteLine("Vous vous déplacez vers le sud");
                    Console.WriteLine();
                    activePlayerM.Move("sud");
                    break;
                case "aller a l'ouest":
                    Console.WriteLine("Vous vous déplacez vers l'ouest");
                    Console.WriteLine();
                    activePlayerM.Move("ouest");
                    break;
                case "aller a l'est":
                    Console.WriteLine("Vous vous déplacez vers l'est");
                    Console.WriteLine();
                    activePlayerM.Move("nord");
                    break;
                case "ou suis-je":
                    Console.WriteLine(activePlayerM.GetCellManager().Cell.Description);
                    break;

                //Options systéme
                case "quitter":
                    returnVal = false;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Arrêt du programme !");
                    break;
                case "aide":
                    GetAvailableCommands();
                    break;
                    /*
                     * Les easter eggs sont cachés un peu de partout, ainsi que les fonctionnalités ajoutées, ici se trouvent
                     * deux petits cheats amusants
                     */
                //CHEATS
                case "all your base are belong to us":
                    using (var db = new Project2NetContext())
                    {
                        var player = (from p in db.Players
                                      where p.Id == activePlayerM.Player.Id
                                      select p).FirstOrDefault();
                        if (player != null)
                        {
                            player.MaxHp = 150 + 150*1000000;
                            player.Hp = player.MaxHp;
                            player.Xp = 100000000;
                            var weaponM = new WeaponManager("2");
                            player.WeaponInventory.Add(weaponM.Weapon);
                        }
                        activePlayerM.Player = player;
                        db.SaveChanges();
                        Console.WriteLine("Tel l'alpha et l'omega, vous devenez grand et puissant.");
                    }
                    break;
                case "mac rules the world":
                    using (var db = new Project2NetContext())
                    {
                        var player = (from p in db.Players
                                      where p.Id == activePlayerM.Player.Id
                                      select p).FirstOrDefault();
                        if (player != null)
                        {
                            var weaponM = new WeaponManager("1");
                            player.WeaponInventory.Add(weaponM.Weapon);
                        }
                        activePlayerM.Player = player;
                        db.SaveChanges();
                        Console.WriteLine("Mais, qu'est ce que c'est que cette grosse chose dans votre fourreau ?");
                    }
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Vous ne pouvez pas faire ceci !");
                    break;
            }
            Console.ResetColor();
            return returnVal;
        }

    }
}
