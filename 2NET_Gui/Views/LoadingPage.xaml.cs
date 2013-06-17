using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2NET_Gui.Helpers;

namespace _2NET_Gui.Views
{
    public partial class LoadingPage : Page
    {
        public LoadingPage()
        {
            InitializeComponent();
            NavigationHelper.MoveToPage(this);
            //Effectuer tous les chargements possiblements bloquants ici.
            MainWindow.SelectedPlayerView = new MainGameView();
            SoundHelper.PlayFromCategory(SoundHelper.Categories.Normal);
            NavigationHelper.MoveToPage(MainWindow.SelectedPlayerView);
        }
    }
}
