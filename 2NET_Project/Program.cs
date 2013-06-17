using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Project.Managers;

/*
 * ASTHENIA GAME
 * Developped by CHEVALIER ALEXIS <123750@supinfo.com>
 * 
 * Notes :
 * Désolé pour les commentaires, ils sont a moitié en anglais et a moitié en francais, c'est nul, mais je n'ai pas eu le temps de tout unifier.
 */

namespace _2NET_Project
{
    class Program
    {
        //Un seul random -> Une seule seed -> Plus de randomness -> awesome
        public static readonly Random Random = new Random();
        static void Main(string[] args)
        {
            
            var introMethods = new Start();
            var game = new Game();
            var active = true;

            //Affichage du début
            var activePlayerM = introMethods.Intro();
            Console.WriteLine(activePlayerM.GetCellManager().Cell.Description);
            while (active) //Main Loop
            {
                Console.WriteLine();
                Console.WriteLine("Que voulez vous faire ? (Tapez Aide)");
                if (!game.CheckCommand(Console.ReadLine(), activePlayerM))
                {
                    active = false;
                }
            }
        }
    }
}
