using System;
using System.Data.Objects;
using System.Linq;
using _2NET_Dal;
using _2NET_Project.Managers;

namespace _2NET_Project
{
    class Start
    {
        //Introduction (menu, toussa toussa)
        public PlayerManager Intro()
        {
            using (var db = new Project2NetContext())
            {
                PlayerManager selectedPlayer = null;

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
                Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████ ██████  ██████  █      █  ███████  ██████  ██      █  ███████  ██████ ████");
                Console.WriteLine("████ █    █  █       █      █     █     █       █ █     █     █     █    █ ████");
                Console.WriteLine("████ █    █  █       █      █     █     █       █  █    █     █     █    █ ████");
                Console.WriteLine("████ ██████  ██████  ████████     █     ██████  █   █   █     █     ██████ ████");
                Console.WriteLine("████ █    █       █  █      █     █     █       █    █  █     █     █    █ ████");
                Console.WriteLine("████ █    █       █  █      █     █     █       █     █ █     █     █    █ ████");
                Console.WriteLine("████ █    █  ██████  █      █     █     ██████  █      ██  ███████  █    █ ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████                      © 2012 - HEAVENSTAR STUDIOS                      ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("████                  Appuyez sur un bouton pour commencer.                ████");
                Console.WriteLine("████                  (Le chargement peut être un peu long)                ████");
                Console.WriteLine("████                                                                       ████");
                Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
                Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
                Console.ResetColor();

                Console.ReadKey();
                var atLeastOneGame = (from players in db.Players select players).Any();
                do
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
                    Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("████                                                                       ████");
                    Console.WriteLine("████ ██████  ██████  █      █  ███████  ██████  ██      █  ███████  ██████ ████");
                    Console.WriteLine("████ █    █  █       █      █     █     █       █ █     █     █     █    █ ████");
                    Console.WriteLine("████ █    █  █       █      █     █     █       █  █    █     █     █    █ ████");
                    Console.WriteLine("████ ██████  ██████  ████████     █     ██████  █   █   █     █     ██████ ████");
                    Console.WriteLine("████ █    █       █  █      █     █     █       █    █  █     █     █    █ ████");
                    Console.WriteLine("████ █    █       █  █      █     █     █       █     █ █     █     █    █ ████");
                    Console.WriteLine("████ █    █  ██████  █      █     █     ██████  █      ██  ███████  █    █ ████");
                    Console.WriteLine("████_______________________________________________________________________████");
                    Console.WriteLine("████                                                                       ████");
                    Console.WriteLine("████ MAIN MENU                                                             ████");
                    Console.WriteLine("████                                                                       ████");
                    Console.WriteLine("████   1- Créer un nouveau personnage                                      ████");
                    if (atLeastOneGame)
                    {
                        Console.WriteLine(
                            "████   2- Charger un personnage                                            ████");
                        Console.WriteLine(
                            "████   3- A Propos                                                         ████");
                        Console.WriteLine(
                            "████   4- Quitter                                                          ████");
                    }
                    else
                    {

                        Console.WriteLine(
                            "████   2- A Propos                                                         ████");
                        Console.WriteLine(
                            "████   3- Quitter                                                          ████");
                        Console.WriteLine(
                            "████                                                                       ████");
                    }


                    Console.WriteLine("████                                                                       ████");
                    Console.WriteLine("████                         Choisissez une option                         ████");
                    Console.WriteLine("████                                                                       ████");
                    Console.WriteLine("████                                                                       ████");
                    Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
                    Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
                    Console.ResetColor();

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            selectedPlayer = NewGame();
                            break;
                        case "2":
                            if (atLeastOneGame)
                            {
                                selectedPlayer = LoadGame();
                            }
                            else
                            {
                                ShowAbout();
                            }
                            break;
                        case "3":
                            if (atLeastOneGame)
                            {
                                ShowAbout();
                            }
                            else
                            {
                                Environment.Exit(0);
                            }
                            break;
                        case "4":
                            if (atLeastOneGame)
                            {
                                Environment.Exit(0);
                            }
                            break;
                    }
                } while (selectedPlayer == null);

                return selectedPlayer;
            }
        }

        //Procédure de chargement d'un player
        public PlayerManager LoadGame()
        {
            using (var db = new Project2NetContext())
            {
                var players = from player in db.Players select player;
                PlayerManager selected;
                do
                {
                    int gameId;
                    do
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Selectionnez l'ID de la partie que vous voulez jouer :");
                        foreach (var player in players)
                        {
                            Console.WriteLine("ID {0} - Nom : {1} - HP : {2} - Level : {3}", player.Id,
                                                            player.Name, player.Hp, (int)Math.Floor((double) player.Xp/100));
                        }
                        Console.ResetColor();
                    } while (!int.TryParse(Console.ReadLine(), out gameId));
                    
                    selected = new PlayerManager(gameId);
                } while (selected.Player == null);
                Console.Clear();
                selected.ShowPlayerInfos();
                return selected;
            }
        }

        //Procédure de nouveau player
        public PlayerManager NewGame()
        {
            using (var db = new Project2NetContext())
            {
                //Création du joueur
                PlayerManager selected;
                do
                {
                    String name = "";
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Choisissez un nom pour votre personnage (1-15 caractéres)");
                        name = Console.ReadLine();
                    } while (name.Length >= 15 || name.Length < 1);
                    selected = new PlayerManager(name);
                } while (selected.Player == null);
                ShowPlot();
                Console.Clear();
                selected.ShowPlayerInfos();
                Console.WriteLine("Aprés avoir marché pendant quelques heures,");

                Console.WriteLine();
                return selected;
            }
        }

        //Texte d'about
        public void ShowAbout()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████              ASHTENIA GAME EST UN PROJET DE C# DE SUPINFO             ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                DEVELOPPE PAR CHEVALIER ALEXIS (123750)                ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                      © 2012 - HEAVENSTAR STUDIOS                      ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████              Appuyez sur un bouton pour retourner au menu             ████");
            Console.WriteLine("████                                                                       ████");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.ResetColor();

            Console.ReadKey();
        }
    
        //Texte d'introduction
        public void ShowPlot()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████  Vous êtes un fermier dans le royaume lointain d'Ashtenia, depuis     ████");
            Console.WriteLine("████  longtemps votre famille cultive ces terres ancestrales qui vous      ████");
            Console.WriteLine("████  appartiennent désormais.                                             ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████  Durant la 4e ére solaire, un mysterieux mal ravage le royaume, les   ████");
            Console.WriteLine("████  villes sont sous la menace constante des créatures qui, autrefois    ████");
            Console.WriteLine("████  pacifiques, se réunissent en hordes pour piller et ravager           ████");
            Console.WriteLine("████  les contrées.                                                        ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████  Avant-hier, le village voisin à été attaqué et pillé, vous vous      ████");
            Console.WriteLine("████  décidez à vous préparer à partir à l'aventure pour défendre le       ████");
            Console.WriteLine("████  royaume.                                                             ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████  Ce matin, vous êtes prêt à vous séparer de votre vie de fermier      ████");
            Console.WriteLine("████  pour devenir aventurier, vous partez donc droit devant vous, vers    ████");
            Console.WriteLine("████  l'inconnu ...                                                        ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("████                  Appuyez sur un bouton pour commencer                 ████");
            Console.WriteLine("████                                                                       ████");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████");
            Console.ResetColor();

            Console.ReadKey();
        }
    }
}