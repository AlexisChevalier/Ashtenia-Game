using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2NET_Dal;
using _2NET_Dal.Model;

namespace _2NET_Gui.Managers
{
    public class ItemManager
    {
        public Item Item;

        public ItemManager(Item item)
        {
            Item = item;
        }

        public String Name
        {
            get
            {
                if (Item != null) return Item.ObjectType.Name;
                return null; 
            }
        }

        public String Level
        {
            get
            {
                if (Item != null) return "Niveau : " + Item.ObjectType.Level;
                return null;
            }
        }

        public String AttackStrenghtBonus
        {
            get
            {
                if (Item != null) return "Bonus Attaque : " + Item.ObjectType.AttackStrenghtBonus;
                return null;
            }
        }

        public String DefenseBoost
        {
            get
            {
                if (Item != null) return "Bonus Défense : " + Item.ObjectType.DefenseBoost;
                return null;
            }
        }

        public String HpRestoreValue
        {
            get
            {
                if (Item != null) return "Bonus Points de vie : " + Item.ObjectType.HpRestoreValue;
                return null;
            }
        }
         
        //Générateur d'item, crée l'itemType si besoin, sinon il le charge, puis redirige vers les autres méthodes
        /* Types d'items :
         *  0 - Health Potion
         *  1 - Attack Potion
         *  2 - Defense Potion
         */   
        public ItemManager(int level, int type = -1)
        {
            Item = new Item();
            var rand = MainWindow.Random;

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
                Item.ObjectType = type;
            }
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
                        AttackStrenghtBonus = 5 + 5 * level,
                        DefenseBoost = 0,
                        HpRestoreValue = 0,
                        Level = level,
                        Name = "Potion de force",
                        Type = 1
                    };
                    db.ItemsTypess.Add(type);
                    db.SaveChanges();
                }
                Item.ObjectType = type;
            }
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
                Item.ObjectType = type;
            }
        }

    }
}
