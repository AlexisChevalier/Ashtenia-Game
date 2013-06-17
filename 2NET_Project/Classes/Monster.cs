namespace _2NET_Project.Classes
{
    /*
     * Monster a été sortie du DAL car je ne vois pas l'intéret de stocker les monstres en DB dans mon interprétation du projet
     * Vu que les monstres sont générés a la volée pendant le combat, et supprimés en suite, il n'y a pas lieu de les mettre en DB
     */
    public class Monster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AttackRate { get; set; }
        public int MissRate { get; set; }
        public int Hp { get; set; }
        public int Damage { get; set; }
        public int Group { get; set; }
    }
}
