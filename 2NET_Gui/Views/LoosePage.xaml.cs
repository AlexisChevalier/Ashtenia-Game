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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Gui.Helpers;
using _2NET_Gui.Managers;

namespace _2NET_Gui.Views
{
    /// <summary>
    /// Logique d'interaction pour LoosePage.xaml
    /// </summary>
    public partial class LoosePage : Page
    {
        public LoosePage()
        {
            InitializeComponent();
        }

        private void StayAlive_Click(object sender, RoutedEventArgs e)
        {
            var db = new Project2NetContext();
            var playerM = MainWindow.SelectedPlayerViewModel.ActivePlayerManager;
            if (playerM.Player != null)
            {
                playerM.Player.Xp = playerM.Player.Xp - 300;
                if (playerM.Player.Xp <= 0) playerM.Player.Xp = 0;
                playerM.Player.MaxHp = 500 + ((int)Math.Floor((double)playerM.Player.Xp / 100)) * 150;
                playerM.Player.Hp = playerM.Player.MaxHp;
                var itemList = playerM.Player.ObjectInventory.ToList();
                foreach (var item in itemList)
                {
                    playerM.Player.ObjectInventory.Remove(item);
                    var itemD = (from i in db.Items
                                 where i.Id == item.Id
                                 select i).FirstOrDefault();
                    db.Items.Remove(itemD);
                }

                var weaponList = playerM.Player.WeaponInventory.ToList();
                foreach (var weapon in weaponList)
                {
                    playerM.Player.WeaponInventory.Remove(weapon);
                    var weaponD = (from i in db.Weapons
                                   where i.Id == weapon.Id
                                   select i).FirstOrDefault();
                    db.Weapons.Remove(weaponD);
                }
                db.SaveChanges();
                var weaponM = new WeaponManager(playerM.Player.Xp, true);
                playerM.Player.WeaponInventory.Add(weaponM.Weapon);
                MainWindow.SelectedPlayerViewModel.ActivePlayerManager.Player = playerM.Player;
                MainWindow.SelectedPlayerViewModel.ActivePlayerManager.Save();
                MainWindow.SelectedPlayerViewModel.Weapons.Clear();
                MainWindow.SelectedPlayerViewModel.Items.Clear();
                var weaponAdded = MainWindow.SelectedPlayerViewModel.ActivePlayerManager.AddWeapon();
                MainWindow.SelectedPlayerViewModel.Weapons.Add(weaponAdded);

                MessageBox.Show("Vous vous sentez Renaitre !");
                MainWindow.SelectedPlayerViewModel.RefreshMainUi();

                SoundHelper.PlayFromCategory(SoundHelper.Categories.Normal);
                NavigationHelper.MoveToPage(MainWindow.SelectedPlayerView);
            }
        }

        private void Die_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Project2NetContext())
            {
                var player = (from p in db.Players
                              where p.Id == MainWindow.SelectedPlayerViewModel.ActivePlayerManager.Player.Id
                              select p).FirstOrDefault();
                if (player != null)
                {
                    db.Players.Remove(player);
                    db.SaveChanges();
                }
            }
            MessageBox.Show("Vous sentez votre force vous abandonner...");
            Environment.Exit(0);
        }
    }
}
