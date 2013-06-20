using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2NET_Dal.Model;
using _2NET_Gui.Classes;

namespace _2NET_Gui.Managers
{
    class MonsterManager
    {
        public Monster Monster { get; set; }
        public int MonsterMaxHp { get; set; }
        public int MonsterLevel { get; set; }

        //Crée un monstre en fonction du joueur et de la case(donc du biome)
        public MonsterManager(Player player, Cell cell)
        {
            var monsterLibrary1 = new List<string>()
                {
                   "Loup enragé",
                   "Ours furieux",
                   "Araignée verte",
                   "Faucon sauvage",
                   "Taureau en rut",
                   "Vipére sournoise",
                   "Bandit de grand chemin",
                   "Chat de chasse",//Utilise ses griffes comme arme de destruction massive
                };
            var monsterLibrary2 = new List<string>()
                {
                  "Rejeton de magie",
                   "Bête de mana",
                   "Sorcier fou",
                   "Mage noir",
                   "Mage des arcanes",
                   "Sorciére sadique",
                   "Playmobil vivant", //Glauque
                };
            var monsterLibrary3 = new List<string>()
                {
                   "Nécromancien",
                   "Cerbére",
                   "Essence de chaos",
                   "Satyre lubrique",
                   "Flamme hurlante",                   
                   "Caniche d'attaque", //Attention, il bave
                };
            var monsterLibrary4 = new List<string>()
                {
                   "Bandit de la ville basse",
                   "Marchand en colére",
                   "Paysan révolté",
                   "Pillard",
                   "Bête des égouts",
                   "Rick Astley", //NEVER GONNA GIVE YOU UP
                };
            var monsterLibrary5 = new List<string>()
                {
                   "Guerrier en décomposition",
                   "Squelette",
                   "Liche",
                   "Zombie purulent",
                   "Monstre de chair", 
                   "Ballmer Sauvage", //lol
                };

            var random = MainWindow.Random;
            string selectedMonster;
            switch (cell.MonsterGroup)
            {
                case 1:
                    selectedMonster = monsterLibrary1[random.Next(0, monsterLibrary1.Count)];
                    break;
                case 2:
                    selectedMonster = monsterLibrary2[random.Next(0, monsterLibrary2.Count)];
                    break;
                case 3:
                    selectedMonster = monsterLibrary3[random.Next(0, monsterLibrary3.Count)];
                    break;
                case 4:
                    selectedMonster = monsterLibrary4[random.Next(0, monsterLibrary4.Count)];
                    break;
                case 5:
                    selectedMonster = monsterLibrary5[random.Next(0, monsterLibrary5.Count)];
                    break;
                default:
                    selectedMonster = monsterLibrary1[random.Next(0, monsterLibrary1.Count)];
                    break;
            }

            var attackRate = 40;
            var missRate = 30;
            var health = 60;
            var damage = 15;
            var level = (int)Math.Floor((double)player.Xp / 100);
            var levelBlocked = level > 10 ? 10 : level;
            MonsterLevel = level;

            attackRate = random.Next((attackRate + (levelBlocked - 1) * 6), (attackRate + levelBlocked * 6));

            missRate = random.Next((missRate - levelBlocked * 3), (missRate - (levelBlocked - 1) * 3));

            health = random.Next((health + (level - 1) * 60), (health + level * 60));
            MonsterMaxHp = health;

            damage = damage + level * 15;

            Monster = new Monster { AttackRate = attackRate, MissRate = missRate, Group = cell.MonsterGroup, Hp = health, Name = selectedMonster, Damage = damage };
        }

        public String GetFormatedLife
        {
            get
            {
                if (Monster.Hp <= 0)
                {
                    return "0 / " + MonsterMaxHp;
                }
                else
                {
                    return Monster.Hp + " / " + MonsterMaxHp;
                }
            }
        }

        public object GetLevel
        {
            get { return "Lvl " + MonsterLevel; }
        }
    }
}
