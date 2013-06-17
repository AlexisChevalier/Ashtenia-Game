using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2NET_Dal;
using _2NET_Dal.Model;

namespace _2NET_Project.Managers
{
    class ItemManager
    {
        public PlayerManager PlayerM { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }

        //Générateur d'item, crée l'itemType si besoin, sinon il le charge, puis redirige vers les autres méthodes
        /* Types d'items :
         *  0 - Health Potion
         *  1 - Attack Potion
         *  2 - Defense Potion
         */
        public ItemManager(int level, PlayerManager playerM, int type =-1)
        {
            var rand = Program.Random;
            PlayerM = playerM;
            Level = level;

            if (type == -1)
            {
                type = rand.Next(3);
            }

            switch (type)
            {
                case 0:
                    GenerateHealthPotion(level);
                    break;
                case 1:
                    GenerateAttackPotion(level);
                    break;
                case 2:
                    GenerateDefensePotion(level);
                    break;
            }
        }

        //génére potion de soin en fonction du lvl
        public void GenerateHealthPotion(int level)
        {
            ItemType type;
            using (var db = new Project2NetContext())
            {
                if ((type = db.ItemsTypess.SingleOrDefault(t => t.Type == 0 && t.Level == level)) == null)
                {
                    type = new ItemType
                    {
                        AttackStrenghtBonus = 0,
                        DefenseBoost = 0,
                        HpRestoreValue = 80 + level * 80,
                        Level = level,
                        Name = "Potion de soins",
                        Type = 0
                    };
                    db.ItemsTypess.Add(type);
                    db.SaveChanges();
                }
            }
            Name = type.Name;
            PlayerM.AddItem(type.Id);
        }

        //Génére potion d'attaque en fnct du lvl
        public void GenerateAttackPotion(int level)
        {
            ItemType type;
            using (var db = new Project2NetContext())
            {
                if ((type = db.ItemsTypess.SingleOrDefault(t => t.Type == 1 && t.Level == level)) == null)
                {
                    type = new ItemType
                    {
                        AttackStrenghtBonus = 5 + 5*level,
                        DefenseBoost = 0,
                        HpRestoreValue = 0,
                        Level = level,
                        Name = "Potion de force",
                        Type = 1
                    };
                    db.ItemsTypess.Add(type);
                    db.SaveChanges();
                }
            }
            Name = type.Name;
            PlayerM.AddItem(type.Id);
        }

        //Génére potion de defense en fnct du lvl
        public void GenerateDefensePotion(int level)
        {
            ItemType type;
            using (var db = new Project2NetContext())
            {
                
                if ((type = db.ItemsTypess.SingleOrDefault(t => t.Type == 2 && t.Level == level)) == null)
                {
                    type = new ItemType
                    {
                        AttackStrenghtBonus = 0,
                        DefenseBoost = 5 + 5 * level,
                        HpRestoreValue = 0,
                        Level = level,
                        Name = "Potion de protection",
                        Type = 2
                    };
                    db.ItemsTypess.Add(type);
                    db.SaveChanges();
                }
            }
            Name = type.Name;
            PlayerM.AddItem(type.Id);
        }
    }
}
