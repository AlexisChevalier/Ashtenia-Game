using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Gui.Extensions;
using _2NET_Gui.Managers;

namespace _2NET_Gui.ViewModels
{
    public class MainGameViewModel : ViewModelBase
    {

        private Player _player;
        private String _selectedPlayerLevel;
        private Weapon _selectedWeapon;
        private Item _selectedItem;
        private ObservableCollection<String> _messageList = new ObservableCollection<String>();
        private ObservableCollection<Cell> _grid = new ObservableCollection<Cell>();
        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private ObservableCollection<Weapon> _weapons = new ObservableCollection<Weapon>();
        private ICommand _moveTo;
        private ICommand _useItem;
        private ICommand _exit;

        public MainGameViewModel()
        {
            _player = MainWindow.SelectedPlayerManager.Player;
            _items = (from i in _player.ObjectInventory where i.ObjectType.Type == 0 select i).ToObservableCollection();
            _weapons = (from i in _player.WeaponInventory select i).ToObservableCollection();

            ObservableCollection<Cell> tempGrid = new ObservableCollection<Cell>();
            tempGrid.Add(new Cell());
           /* using (var db = new Project2NetContext())
            {
                tempGrid = (from cell in db.Cells
                            where (cell.PosX >= _player.CurrentCell.PosX - 2 && cell.PosX <= _player.CurrentCell.PosX + 2)
                             && (cell.PosY >= _player.CurrentCell.PosY-2 && cell.PosY <= _player.CurrentCell.PosY+2)
                         select cell).ToObservableCollection();
            }*/
            for (var a = -2; a <= 2; a++)
            {
                for (var b = -2; b <= 2; b++)
                {
                    /*var temp = tempGrid.SingleOrDefault(
                        cell => (cell.PosX == _player.CurrentCell.PosX + a) && (cell.PosY == _player.CurrentCell.PosY + b));*/
                    Cell temp = null;
                    
                    if (temp == null)
                    {
                        temp = new Cell {
                            PosX = _player.CurrentCell.PosX + a,
                            PosY = _player.CurrentCell.PosX + b,
                            Description = "Case Inexplorée",
                            MonsterGroup = -1,
                            ImageSource = "../Ressources/Images/ground_empty_cell.png",
                        };
                       
                    }
                    
                    _grid.Add(temp);
                }
            }
        }

        public ObservableCollection<Cell> Grid
        {
            get { return _grid; }
        }

        public ObservableCollection<Item> Items
        {
            get { return _items; }
        }

        public ObservableCollection<Weapon> Weapons
        {
            get { return _weapons; }
        }

        public ObservableCollection<String> MessageList
        {
            get { return _messageList; }
        }

        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        public Weapon SelectedWeapon
        {
            get { return _selectedWeapon; }
            set
            {
                _selectedWeapon = value;
                NotifyPropertyChanged("SelectedWeapon");
            }
        }
    }
}
