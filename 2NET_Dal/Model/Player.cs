using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace _2NET_Dal.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Xp { get; set; }
        public int Hp { get; set; }
        
        public int MaxHp { get; set; }
        public virtual ICollection<Weapon> WeaponInventory { get; set; }
        public virtual ICollection<Item> ObjectInventory { get; set; }

        public int? CurrentCellId { get; set; }

        public Cell CurrentCell { get; set; }
    }
}
