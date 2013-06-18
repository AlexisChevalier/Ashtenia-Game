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
using _2NET_Gui.Helpers;
using _2NET_Gui.Managers;
using _2NET_Gui.Views;

namespace _2NET_Gui.ViewModels
{
    public class MainGameViewModel : ViewModelBase
    {
        /* DECLARATIONS */
        private PlayerManager _playerM;
        private Weapon _selectedWeapon;
        private Item _selectedItem;
        private ObservableCollection<String> _messageList = new ObservableCollection<String>();
        private ObservableCollection<Cell> _grid = new ObservableCollection<Cell>();
        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private ObservableCollection<Weapon> _weapons = new ObservableCollection<Weapon>();
        private ICommand _moveToTop;
        private ICommand _moveToBottom;
        private ICommand _moveToLeft;
        private ICommand _moveToRight;
        private ICommand _useItem;
        private ICommand _dropItem;
        private ICommand _dropWeapon;
        private ICommand _searchInZone;
        private ICommand _exit;

        /* CONSTRUCTOR */
        public MainGameViewModel()
        {
            _playerM = MainWindow.SelectedPlayerManager;
            _items = _playerM.GetItems();
            _weapons = _playerM.GetWeapons();
            ExecRefreshGrid();
        }

        /* ACCESSORS */
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
            set
            {
                _weapons = value;
                NotifyPropertyChanged("Weapons");
            }
        }

        public ObservableCollection<String> MessageList
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                NotifyPropertyChanged("MessageList");
            }
        }

        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
                NotifyPropertyChanged("SelectedItemManager");
            }
        }

        public ItemManager SelectedItemManager
        {
            get { return new ItemManager(_selectedItem); }
        }

        public Weapon SelectedWeapon
        {
            get { return _selectedWeapon; }
            set
            {
                _selectedWeapon = value;
                NotifyPropertyChanged("SelectedWeapon");
                NotifyPropertyChanged("SelectedWeaponManager");
            }
        }

        public WeaponManager SelectedWeaponManager 
        {
            get { return new WeaponManager(SelectedWeapon); }
        }

        public PlayerManager ActivePlayerManager
        {
            get { return _playerM; }
        }


        /* COMMANDS */


        public ICommand MoveToTop
        {
            get
            {
                if (_moveToTop == null)
                {
                    _moveToTop = new CommandHelper(param => this.ExecMoveTo("nord"),
                        null);
                }
                return _moveToTop;
            }
        }
        public ICommand MoveToBottom
        {
            get
            {
                if (_moveToBottom == null)
                {
                    _moveToBottom = new CommandHelper(param => this.ExecMoveTo("sud"),
                        null);
                }
                return _moveToBottom;
            }
        }
        public ICommand MoveToLeft
        {
            get
            {
                if (_moveToLeft == null)
                {
                    _moveToLeft = new CommandHelper(param => this.ExecMoveTo("ouest"),
                        null);
                }
                return _moveToLeft;
            }
        }
        public ICommand MoveToRight
        {
            get
            {
                if (_moveToRight == null)
                {
                    _moveToRight = new CommandHelper(param => this.ExecMoveTo("est"),
                        null);
                }
                return _moveToRight;
            }
        }

        public ICommand SearchInZone
        {
            get
            {
                if (_searchInZone == null)
                {
                    _searchInZone = new CommandHelper(param => this.ExecSearchInZone(),
                        CanSearchInZone);
                }
                return _searchInZone;
            }
        }
        public bool CanSearchInZone(object o)
        {
            var cellM = new CellManager((int)_playerM.Player.CurrentCellId);
            return !cellM.HadBeenVisited();
        }

        public ICommand UseItem
        {
            get
            {
                if (_useItem == null)
                {
                    _useItem = new CommandHelper(param => this.ExecUseItem(),
                        CanUseItem);
                }
                return _useItem;
            }
        }
        public bool CanUseItem(object o)
        {
            return _selectedItem != null && _selectedItem.ObjectType.Type == 0;
        }

        public ICommand DropItem
        {
            get
            {
                if (_dropItem == null)
                {
                    _dropItem = new CommandHelper(param => this.ExecDropItem(),
                        CanDropItem);
                }
                return _dropItem;
            }
        }
        public bool CanDropItem(object o)
        {
            return _selectedItem != null;
        }

        public ICommand DropWeapon
        {
            get
            {
                if (_dropWeapon == null)
                {
                    _dropWeapon = new CommandHelper(param => this.ExecDropWeapon(),
                        CanDropWeapon);
                }
                return _dropWeapon;
            }
        }
        public bool CanDropWeapon(object o)
        {
            return SelectedWeapon != null && Weapons.Count != 1;
        }

        /* EXECUTIONS */

        public void ExecDropItem()
        {
            ActivePlayerManager.DropItem(SelectedItemManager.Item.Id);
            Items.Remove(SelectedItem);
            SelectedItem = null;
            NotifyPropertyChanged("Items");
        }

        public void ExecDropWeapon()
        {
            ActivePlayerManager.DropWeapon(SelectedWeaponManager.Weapon.Id);
            Weapons.Remove(SelectedWeapon);
            SelectedWeapon = null;
            NotifyPropertyChanged("Weapons");
        }

        public void ExecUseItem()
        {
            var message = ActivePlayerManager.UseItem(SelectedItemManager.Item);
            MessageList.Insert(0, message);
            if (message != "Vous ne pouvez utiliser que des potions de soins hors combat !")
            {
                Items.Remove(SelectedItem);
                SelectedItem = null;
                NotifyPropertyChanged("Items");
                NotifyPropertyChanged("ActivePlayerManager");
            }
        }

        public void ExecSearchInZone()
        {
            
            var cellM = new CellManager((int)_playerM.Player.CurrentCellId);
            if (cellM.HadBeenVisited())
            {
                MessageBox.Show("Vous avez déja fouillé cette case, voyons !");
            }
            else
            {
                cellM.IsVisited();
                var rand = MainWindow.Random;
                if (rand.Next(0, 101) > 90) //10 % de chances de trouver un item
                {
                    var item = ActivePlayerManager.AddItem();
                    MessageList.Insert(0, String.Format("=> {0} - Niveau {1}", item.ObjectType.Name, item.ObjectType.Level));
                    MessageList.Insert(0, "Quelle chance ! Vous avez trouvé un objet !");
                    Items.Add(item);
                }
                else if (rand.Next(0, 101) > 95) //5% de trouver une arme
                {
                    var weapon = ActivePlayerManager.AddWeapon();
                    MessageList.Insert(0, String.Format("=> {0} - Niveau {1}", weapon.Name, weapon.Level));
                    MessageList.Insert(0, "Petit veinard ! Vous avez trouvé une arme !");
                    Weapons.Add(weapon);
                }
                else
                {
                    MessageList.Insert(0,"Vous n'avez rien trouvé !");
                }
            }
        }

        public void ExecMoveTo(string direction)
        {
            var cellM = new CellManager((int)_playerM.Player.CurrentCellId);
            var canMoveTo = cellM.ToArrayCanMoveTo();
            var index = -1;
            switch (direction)
            {
                case "nord":
                    index = 3;
                    break;
                case "est":
                    index = 0;
                    break;
                case "sud":
                    index = 1;
                    break;
                case "ouest":
                    index = 2;
                    break;
                default:
                    return;
            }
            if (canMoveTo[index] == '1')
            {
                cellM = new CellManager(cellM.Cell.PosX, cellM.Cell.PosY, index);
                /* Va-t-il y avoir un combat ?*/
                var rand = MainWindow.Random;


                if (rand.Next(0, 101) < cellM.Cell.MonsterRate) //YEAAAAAAH CA VA CHAUFFER A DONF 
                {
                    _playerM.Player.CurrentCellId = cellM.Cell.Id;
                    MessageList.Insert(0, cellM.Cell.Description);
                    _playerM.Save();
                    ExecRefreshGrid();

                    MainWindow.ActiveCombatpage = new CombatPage();

                    MessageBox.Show("Vous avez rencontré un monstre, le combat va commencer !");

                    NavigationHelper.MoveToPage(MainWindow.ActiveCombatpage);
                }
                else
                {
                    _playerM.Player.CurrentCellId = cellM.Cell.Id;
                    MessageList.Insert(0, cellM.Cell.Description);
                    _playerM.Save();
                    ExecRefreshGrid();
                }
            }
            else
            {
                MessageList.Insert(0,"Vous ne trouvez pas de chemin pour aller ici !");
            }
        }

        public void ExecRefreshGrid()
        {
            _grid.Clear();
            ObservableCollection<Cell> tempGrid;
            using (var db = new Project2NetContext())
            {
                _playerM.Player.CurrentCell = db.Cells.FirstOrDefault(cell => cell.Id == (int)_playerM.Player.CurrentCellId);
                tempGrid = (from cell in db.Cells
                            where (cell.PosX >= _playerM.Player.CurrentCell.PosX - 2 && cell.PosX <= _playerM.Player.CurrentCell.PosX + 2)
                             && (cell.PosY >= _playerM.Player.CurrentCell.PosY - 2 && cell.PosY <= _playerM.Player.CurrentCell.PosY + 2)
                            select cell).ToObservableCollection();
            }
            for (var a = -2; a <= 2; a++)
            {
                for (var b = -2; b <= 2; b++)
                {
                    var temp = tempGrid.SingleOrDefault(
                        cell => (cell.PosX == _playerM.Player.CurrentCell.PosX + a) && (cell.PosY == _playerM.Player.CurrentCell.PosY + b));

                    if (temp == null)
                    {
                        temp = new Cell
                        {
                            PosX = _playerM.Player.CurrentCell.PosX + a,
                            PosY = _playerM.Player.CurrentCell.PosX + b,
                            Description = "Case Inexplorée",
                            MonsterGroup = -1,
                            ImageSource = "../Ressources/Images/ground_empty_cell.png",
                            CanMoveTo = "0000",
                            Id = 0,
                            MonsterRate = 0,
                            Visited = false
                        };
                    }
                    _grid.Add(temp);
                }
            }
            NotifyPropertyChanged("Grid");
        }
    }
}