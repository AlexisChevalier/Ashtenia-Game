using System.Data.Entity;

namespace _2NET_Dal.Model
{
    public class Item
    {
        public int Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual ItemType ObjectType { get; set; }
    }
}
