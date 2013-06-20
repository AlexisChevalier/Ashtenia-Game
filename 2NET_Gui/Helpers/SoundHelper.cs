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
            Defeat
        }
        private static readonly SoundPlayer Player = new SoundPlayer();
        private static Thread _soundThread;
        private static int _lastNormalPlayed = 1;
        private static int _lastFightPlayed = 1;

        public static void PlayFromCategory(Categories categorie)
        {
            Stop();
            var soundLocation = "";
            var random = MainWindow.Random.Next(0, 100);
            switch (categorie)
            {
                case Categories.Intro:
                    soundLocation = @"medias\sound\INTRO1.wav";
                    break;
                case Categories.Normal:
                    if (_lastNormalPlayed == 1)
                    {
                        _lastNormalPlayed = 2;
                        soundLocation = @"medias\sound\NORMAL2.wav";
                    }
                    else
                    {
                        _lastNormalPlayed = 1;
                        soundLocation = @"medias\sound\NORMAL1.wav";
                    }
                    break;
                case Categories.Fight:
                    if (_lastFightPlayed == 1)
                    {
                        _lastFightPlayed = 2;
                        soundLocation = @"medias\sound\FIGHT2.wav";
                    }
                    else
                    {
                        _lastFightPlayed = 1;
                        soundLocation = @"medias\sound\FIGHT1.wav";
                    }
                    break;
                case Categories.Defeat:
                    soundLocation = @"medias\sound\DEFEAT.wav";
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
