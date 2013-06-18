using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2NET_Gui.Helpers;
using _2NET_Gui.Managers;
using _2NET_Gui.Views;

namespace _2NET_Gui
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly Random Random = new Random();
        public static PlayerManager SelectedPlayerManager;
        public static MainGameView SelectedPlayerView;
        public static CombatPage ActiveCombatpage;


        public MainWindow()
        {
            Content = new HomePage();
            InitializeComponent();
            SoundHelper.PlayFromCategory(SoundHelper.Categories.Intro);
        }
    }
}
