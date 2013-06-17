using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _2NET_Gui.Helpers
{
    static class SoundHelper
    {
        public enum Categories
        {
            Intro,
            Normal,
            Fight,
            Defeat,
            Victory
        }
        private static readonly SoundPlayer Player = new SoundPlayer();
        private static Thread _soundThread;

        public static void PlayFromCategory(Categories categorie)
        {
            Stop();
            var soundLocation = "";
            var random = MainWindow.Random.Next(0, 2);
            switch (categorie)
            {
                case Categories.Intro:
                    soundLocation = @"medias\sound\INTRO1.wav";
                    break;
                case Categories.Normal:
                    soundLocation = @"medias\sound\NORMAL1.wav";
                    break;
                case Categories.Fight:
                    soundLocation = @"medias\sound\FIGHT1.wav";
                    break;
                case Categories.Defeat:
                    soundLocation = @"medias\sound\DEFEAT.wav";
                    break;
                case Categories.Victory:
                    soundLocation = @"medias\sound\VICTORY.wav";
                    break;
            }
            Player.SoundLocation = soundLocation;
            Player.Load();
            Player.PlayLooping();
        }

        public static void Stop()
        {
            Player.Stop();
        }
    }
}
