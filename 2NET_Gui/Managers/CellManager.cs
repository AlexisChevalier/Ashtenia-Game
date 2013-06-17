using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Gui.Classes;

namespace _2NET_Gui.Managers
{
    public class CellManager
    {
        public Cell Cell { get; set; }

        //Nouvelle Cellule
        public CellManager(int x, int y, int direction = -1)
        {
            using (var db = new Project2NetContext())
            {
                //Random Generator
                var random = MainWindow.Random;
                //Nouvelles positions
                var newX = new int();
                var newY = new int();
                //Initialisation d'une chaine de chars
                var canMoveTo = new char[4]; //nord;est;sud;ouest
                //Index du tableau qui sera fixé
                var index = new int();
                //Case possiblement bloquée
                var randomCell = new int();

                switch (direction)
                {
                    case -1: //Départ
                        newX = x;
                        newY = y;
                        index = random.Next(0, 4);
                        canMoveTo[index] = '1'; //Case forcément ouverte
                        break;
                    case 0: //nord
                        newX = x;
                        newY = y + 1;
                        index = 2;
                        canMoveTo[index] = '1'; //Case forcément ouverte
                        break;
                    case 1: //est
                        newX = x + 1;
                        newY = y;
                        index = 3;
                        canMoveTo[index] = '1'; //Case forcément ouverte
                        break;
                    case 2: //sud
                        newX = x;
                        newY = y - 1;
                        index = 0;
                        canMoveTo[index] = '1'; //Case forcément ouverte
                        break;
                    case 3: //ouest
                        newX = x - 1;
                        newY = y;
                        index = 1;
                        canMoveTo[index] = '1'; //Case forcément ouverte
                        break;
                }
                var existingCell = db.Cells.SingleOrDefault(cell => cell.PosX == newX && cell.PosY == newY);
                if (existingCell == null)
                {
                    //Choix d'une case possiblement bloquée
                    do
                    {
                        randomCell = random.Next(0, 4);
                    } while (randomCell == index);
                    //remplissage de l'array de possibilités
                    for (var a = 0; a < 4; a++)
                    {
                        if (a == randomCell) //Si c'est la case possiblement bloquée
                        {
                            if (random.Next(0, 2) == 1)
                            {
                                canMoveTo[a] = '1';
                            }
                            else
                            {
                                canMoveTo[a] = '0';
                            }
                        }
                        else if (a != index) //Si c'est pas la case forcément ouverte
                        {
                            canMoveTo[a] = '1';
                        }
                    }

                    //Min 30 Max 50% de mob rate
                    var monsterRate = random.Next(0, 51);
                    Biome biome = null;

                    biome = GetRandomBiome();

                    Cell = new Cell
                    {
                        Description = biome.Description,
                        ImageSource = biome.ImageSource,
                        CanMoveTo = ToStringCanMoveTo(canMoveTo),
                        MonsterGroup = biome.MonsterGroup,
                        MonsterRate = monsterRate,
                        PosX = newX,
                        PosY = newY
                    };
                    db.Cells.Add(Cell);
                    db.SaveChanges();
                }
                else
                {
                    Cell = existingCell;
                }
            }

        }

        //Cellule existante
        public CellManager(int id)
        {
            using (var db = new Project2NetContext())
            {
                Cell = db.Cells.SingleOrDefault(cell => cell.Id == id);
            }
        }

        //Marque la cellule comme fouillée
        public void IsVisited()
        {
            using (var db = new Project2NetContext())
            {
                var cell = (from c in db.Cells
                            where c.Id == Cell.Id
                            select c).FirstOrDefault();
                if (cell != null)
                {
                    cell.Visited = true;
                }

                db.SaveChanges();
            }
            Cell.Visited = true;
        }

        //Dit si oui ou non la cellule a été fouillée
        public bool HadBeenVisited()
        {
            return Cell.Visited;
        }

        //Cellule existante par paramétre
        public CellManager(Cell cell)
        {
            Cell = cell;
        }

        //Return random biome
        public Biome GetRandomBiome()
        {
            return new Biome();
        }

        //Return Array for can move to
        public String ToStringCanMoveTo(char[] array)
        {
            return array.Aggregate("", (current, t) => current + t);
        }

        //Return Array of chars ('1' or '0') for position nord, est, sud, ouest
        public char[] ToArrayCanMoveTo()
        {
            return Cell.CanMoveTo.ToCharArray();
        }
    }
}
