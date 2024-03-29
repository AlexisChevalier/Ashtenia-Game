﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2NET_Gui.Helpers;

namespace _2NET_Gui.Views
{
    /// <summary>
    /// Logique d'interaction pour HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly Page _playerSelectionView;
        public HomePage()
        {
            InitializeComponent();
            _playerSelectionView = new PlayerSelectionView();
        }
        
        void Label_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            NavigationHelper.MoveToPage(_playerSelectionView); 
        }
    }
}
