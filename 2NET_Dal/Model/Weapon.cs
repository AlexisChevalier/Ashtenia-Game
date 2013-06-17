using System.Data.Entity;

namespace _2NET_Dal.Model
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AttackRate { get; set; }
        public int MissRate { get; set; }
        public int Damage { get; set; }
        public int Level { get; set; }
        public Player Player { get; set; }
    }
}
