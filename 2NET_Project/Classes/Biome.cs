using System;
using System.Collections.Generic;

namespace _2NET_Project.Classes
{
    /*
     * les biomes permettent de définir un type de terrain lié a un type de monstre
     */
    public class Biome
    {
        public String Description { get; set; }
        public int MonsterGroup { get; set; }

        //Un biome
        private class BiomeEntry
        {
            public string Description { get; set; }
            public int MonsterGroup { get; set; }
        }

        //génére un biome
        public Biome()
        {

            var biomesLibrary = new List<BiomeEntry>()
                {
                    //Nature
                    new BiomeEntry{Description = "Vous voyagez dans une paisible forêt", MonsterGroup = 1},
                    new BiomeEntry{Description = "Vous passez prés d'un lac", MonsterGroup = 1},
                    new BiomeEntry{Description = "Vous grimpez sur une colline verdoyante", MonsterGroup = 1},
                    new BiomeEntry{Description = "Vous traversez le col d'une montagne", MonsterGroup = 1},
                    new BiomeEntry{Description = "Vous arrivez sur une côte, vous la longez pendant quelques temps", MonsterGroup = 1},

                    //Magic
                    new BiomeEntry{Description = "Vous pénetrez dans une forêt mystique", MonsterGroup = 2},
                    new BiomeEntry{Description = "Vous traversez une riviére enchantée", MonsterGroup = 2},
                    new BiomeEntry{Description = "Vous découvrez une cité débordante d'énergie, ou habitent de nombreux mages", MonsterGroup = 2},
                    new BiomeEntry{Description = "Vous traversez un tunnel regorgeant de puissances inconnues", MonsterGroup = 2},

                    //Chaos
                    new BiomeEntry{Description = "Vous découvrez une citée ravagée, il ne reste que des ruines enflammées", MonsterGroup = 3},
                    new BiomeEntry{Description = "Vous traversez une plaine aride, la chaleur vous écrase", MonsterGroup = 3},
                    new BiomeEntry{Description = "Vous contournez un marais à l'odeur nauséabonde", MonsterGroup = 3},
                    new BiomeEntry{Description = "Vous escaladez une montagne aux sommets sombres et orageux", MonsterGroup = 3},

                    //Cities
                    new BiomeEntry{Description = "Vous traversez une citée pleine de vie, avec de nombreux marchands", MonsterGroup = 4},
                    new BiomeEntry{Description = "Vous pénetrez dans une petite bourgade paisible", MonsterGroup = 4},

                    //UDs
                    new BiomeEntry{Description = "Vous déambulez dans de sombres souterrains", MonsterGroup = 5},
                    new BiomeEntry{Description = "Vous explorez une ville en ruines, il ne reste rien de vivant", MonsterGroup = 5},
                };
            var random = Program.Random;
            var selectedBiome = biomesLibrary[random.Next(0, biomesLibrary.Count)];
            Description = selectedBiome.Description;
            MonsterGroup = selectedBiome.MonsterGroup;
        }
    }
}
