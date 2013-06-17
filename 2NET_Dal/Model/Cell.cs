using System.Data.Entity;

namespace _2NET_Dal.Model
{
    public class Cell
    {
        public int Id { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int MonsterRate { get; set; }
        public int MonsterGroup { get; set; }
        public bool Visited { get; set; }
        public string CanMoveTo { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
    } 
}
