using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _2NET_Gui.Helpers
{
    public static class NavigationHelper
    {
        private static readonly Window Window = Application.Current.MainWindow;

        public static void MoveToPage(Page page)
        {
            Window.Content = page;
        }
    }
}
