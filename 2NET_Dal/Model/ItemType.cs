using System.Data.Entity;

namespace _2NET_Dal.Model
{
    public class ItemType
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int HpRestoreValue { get; set; }
        public int DefenseBoost { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int AttackStrenghtBonus { get; set; }
    }
}
