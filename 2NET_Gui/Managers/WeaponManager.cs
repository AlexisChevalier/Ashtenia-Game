using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2NET_Dal.Model;

namespace _2NET_Gui.Managers
{
    public class WeaponManager
    {
        public Weapon Weapon;

        public String Name
        {
            get { return Weapon.Name;  }
        }

        public String Level
        {
            get { return "Niveau : " + Weapon.Level; }
        }

        public String MissRate
        {
            get { return "Chance rater : " + Weapon.MissRate; }
        }

        public String AttackRate
        {
            get { return "Vitesse attaque : " + Weapon.AttackRate; }
        }

        //Give a weapon with an existing weapon
        public WeaponManager(Weapon weapon)
        {
            Weapon = weapon;
        }

        //Fournit une arme selon le cheat utilisé
        public WeaponManager(string cheat)
        {
            if (cheat == "1")
            {
                Weapon = new Weapon { AttackRate = 105, MissRate = 0, Name = "LOOKS LIKE IT'S GONNA HURT", Damage = 999999, Level = 9999 };
            }
            if (cheat == "2")
            {
                Weapon = new Weapon { AttackRate = 95, MissRate = 2, Name = "YU NO HAVE SHAME ?", Damage = 15 + 1000000 * 15, Level = 1000000 };
            }
           
        }

        //Give a weapon based on experience
        public WeaponManager(int exp, bool first = false)
        {
            var weaponNamesF = new List<string>
                {
                    "Epée",
                    "Massue",
                    "Dague",
                    "Hachette",
                    "Hache",
                    "Pique"
                };

            var weaponNamesM = new List<string>
                {
                    "Gourdin",
                    "Windows Vista",
                    "Nunchaku",
                    "Bâton",
                    "Fléau",
                };

            var weaponAdjectivesF = new List<string>
                {
                    "puissante",
                    "batarde",
                    "légère",
                    "large",
                    "violente",
                    "meurtiére"
                };

            var weaponAdjectivesM = new List<string>
                {
                    "puissant",
                    "léger",
                    "large",
                    "violent",
                    "meurtier",
                    "assommant"
                };

            var weaponSuffixes = new List<string>
                {
                    "de windows millenium",
                    "de mac os",
                    "de java",
                    "du 3310",
                    "du ballmer",
                    "de bris-os",
                    "d'ashtenia",
                    "du meurpog",
                    "du soldat",
                    "du chaos",
                    "des arcanes",
                    "de la nature"
                };

            var attackRate = 35;
            var missRate = 32;
            var rand = MainWindow.Random;
            var level = (int) Math.Floor((double) exp/100);
            var damage = 15;
            var weaponLevel = level;
            damage = damage + level*15;
            level = level > 10 ? 10 : level;

            attackRate = rand.Next((attackRate + (level - 1)*6), (attackRate + level*6));

            missRate = rand.Next((missRate - level*3), (missRate - (level - 1)*3));

            var name = "";

            if (rand.Next(0, 2) == 0)
            {
                name = string.Format("{0} {1} {2}", weaponNamesF[rand.Next(0, weaponNamesF.Count())],
                                     weaponAdjectivesF[rand.Next(0, weaponAdjectivesF.Count())],
                                     weaponSuffixes[rand.Next(0, weaponSuffixes.Count())]);
            }
            else
            {
                name = string.Format("{0} {1} {2}", weaponNamesM[rand.Next(0, weaponNamesM.Count())],
                                     weaponAdjectivesM[rand.Next(0, weaponAdjectivesM.Count())],
                                     weaponSuffixes[rand.Next(0, weaponSuffixes.Count())]);
            }
            if (first == true)
            {
                name = "Couteau rouillé";
            }
            Weapon = new Weapon
                {
                    AttackRate = attackRate,
                    MissRate = missRate,
                    Name = name,
                    Damage = damage,
                    Level = weaponLevel
                };
        }
    }
}
