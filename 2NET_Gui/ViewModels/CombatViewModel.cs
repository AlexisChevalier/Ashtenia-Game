using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Gui.Helpers;
using _2NET_Gui.Managers;

namespace _2NET_Gui.ViewModels
{
    class CombatViewModel : ViewModelBase
    {
        private MonsterManager _activeMonsterManager;
        private ObservableCollection<String> _messageList = new ObservableCollection<String>();
        private ICommand _attackMonster;
        private ICommand _useItem;

        private int _playerAttackBonus = 0;
        private int _monsterAttackBonus = 0;
        private int _playerDefenseBonus = 0;
        private int _monsterDefenseBonus = 0;
        private bool _playerWon = false;
        private bool _playerLoose = false;

        public MainGameViewModel MainGameViewModel
        {
            get { return MainWindow.SelectedPlayerViewModel; }
        }

        public CombatViewModel()
        {
            MainGameViewModel.SelectedWeapon = null;
            MainGameViewModel.SelectedItem = null;
            using (var db = new Project2NetContext())
            {
                _activeMonsterManager = new MonsterManager(MainGameViewModel.ActivePlayerManager.Player, db.Cells.FirstOrDefault(c => c.Id == MainGameViewModel.ActivePlayerManager.Player.CurrentCellId));
            }
        }

        public MonsterManager ActiveMonster
        {
            get { return _activeMonsterManager; }
            set 
            { 
                _activeMonsterManager = value;
                NotifyPropertyChanged("ActiveMonster");
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

        public ICommand AttackMonster
        {
            get
            {
                if (_attackMonster == null)
                {
                    _attackMonster = new CommandHelper(param => this.ExecAttackMonster(),
                        CanAttackMonster);
                }
                return _attackMonster;
            }
        }
        public bool CanAttackMonster(object o)
        {
            return MainGameViewModel.SelectedWeapon != null;
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
            return MainGameViewModel.SelectedItem != null;
        }

        public void ExecAttackMonster()
        {
            var rand = MainWindow.Random;
            PerformAttackMonster();
            //Sommes nous assez rapide ?
            if (ActiveMonster.Monster.AttackRate > MainGameViewModel.SelectedWeapon.AttackRate && !_playerWon && !_playerLoose)
            {
                if (rand.Next(100) > 60)
                {//Oui, on peut frapper une deuxiéme fois avec la même arme
                    MessageList.Insert(0, "Vous avez été suffisement rapide pour donner un second coup !");
                    PerformAttackMonster();
                }
            }

            PerformAttackPlayer();
            //Le monstre est-il assez rapide ?
            if (MainGameViewModel.SelectedWeapon.AttackRate > ActiveMonster.Monster.AttackRate && !_playerWon && !_playerLoose)
            {
                if (rand.Next(100) > 60)
                {//Oui, le monstre peut frapper une deuxiéme fois
                    MessageList.Insert(0, "Le monstre à été suffisement rapide pour donner un second coup !");
                    PerformAttackMonster();
                }
            }
        }

        public void ExecUseItem()
        {
            if (MainGameViewModel.SelectedItem.ObjectType.Type == 0) //Health Potion
            {
                //Soin
                var hp = MainGameViewModel.ActivePlayerManager.Player.Hp;
                MainGameViewModel.ActivePlayerManager.Player.Hp +=
                    MainGameViewModel.SelectedItem.ObjectType.HpRestoreValue;
                if (MainGameViewModel.ActivePlayerManager.Player.Hp > MainGameViewModel.ActivePlayerManager.Player.MaxHp)
                    MainGameViewModel.ActivePlayerManager.Player.Hp = MainGameViewModel.ActivePlayerManager.Player.MaxHp;
                hp = MainGameViewModel.ActivePlayerManager.Player.Hp - hp;
                MessageList.Insert(0,String.Format("L'item {0} vous à soigné pour {1} Hp !", MainGameViewModel.SelectedItem.ObjectType.Name, hp));
            }
            else
            {
                if (MainGameViewModel.SelectedItem.ObjectType.AttackStrenghtBonus != 0)
                {
                    MessageList.Insert(0,String.Format("L'item {0} vous à rendu plus fort de {1} Points !", MainGameViewModel.SelectedItem.ObjectType.Name, MainGameViewModel.SelectedItem.ObjectType.AttackStrenghtBonus));
                    _playerAttackBonus += MainGameViewModel.SelectedItem.ObjectType.AttackStrenghtBonus;
                }

                if (MainGameViewModel.SelectedItem.ObjectType.DefenseBoost != 0)
                {
                    MessageList.Insert(0,String.Format("L'item {0} vous à rendu plus résistant de {1} Points !", MainGameViewModel.SelectedItem.ObjectType.Name, MainGameViewModel.SelectedItem.ObjectType.DefenseBoost));
                    _playerDefenseBonus += MainGameViewModel.SelectedItem.ObjectType.DefenseBoost;
                }
            }

            //Suppression
            using (var db = new Project2NetContext())
            {
                var player = (from p in db.Players
                              where p.Id == MainGameViewModel.ActivePlayerManager.Player.Id
                              select p).FirstOrDefault();
                if (player != null)
                {
                    player.Hp = MainGameViewModel.ActivePlayerManager.Player.Hp;
                    player.MaxHp = MainGameViewModel.ActivePlayerManager.Player.MaxHp;
                    player.Xp = MainGameViewModel.ActivePlayerManager.Player.Xp;
                    player.CurrentCellId = MainGameViewModel.ActivePlayerManager.Player.CurrentCellId;
                    player.ObjectInventory.Remove(MainGameViewModel.SelectedItem);
                    //Copie en local
                    MainGameViewModel.ActivePlayerManager.Player.ObjectInventory = player.ObjectInventory;

                    var item = (from i in db.Items
                                where i.Id == MainGameViewModel.SelectedItem.Id
                                select i).FirstOrDefault();
                    db.Items.Remove(item);
                }

                db.SaveChanges();
                MainGameViewModel.Items.Remove(MainGameViewModel.SelectedItem);
                MainGameViewModel.SelectedItem = null;
                NotifyPropertyChanged("Items");
                NotifyPropertyChanged("ActivePlayerManager");
            }
        }

        /* Combat Methods */

        public void PerformAttackPlayer()
        {
            var rand = MainWindow.Random;
            var missChances = rand.Next(0, 101);
            if (missChances > (100 - ActiveMonster.Monster.MissRate))
            {
                MessageList.Insert(0, "Vous avez raté votre coup !");
            }
            else
            {
                int damage = rand.Next(ActiveMonster.Monster.Damage - 5, ActiveMonster.Monster.Damage + 6);
                damage = damage + _monsterAttackBonus - _playerDefenseBonus;
                if (damage <= 0) damage = 0;
                MainGameViewModel.ActivePlayerManager.Player.Hp -= damage;
                MainGameViewModel.ActivePlayerManager.Save();
                if (ActiveMonster.Monster.Hp <= 0)
                {
                    _playerWon = true;
                    MessageList.Insert(0, String.Format("Vous frappez {0} pour {1} dégats, il meurt.", ActiveMonster.Monster.Name, damage));
                }
                else
                {
                    MessageList.Insert(0, String.Format("Vous frappez {0} pour {1} dégats, il lui reste {2} PVs.", ActiveMonster.Monster.Name, damage, ActiveMonster.Monster.Hp));
                }
                NotifyPropertyChanged("ActivePlayerManager");
                NotifyPropertyChanged("ActiveMonster");
            }
        }

        public void PerformAttackMonster()
        {
            var rand = MainWindow.Random;
            var missChances = rand.Next(0, 101);
            if (missChances > (100 - MainGameViewModel.SelectedWeapon.MissRate))
            {
                MessageList.Insert(0, String.Format("{0} à raté son coup !", ActiveMonster.Monster.Name));
            }
            else
            {
                int damage = rand.Next(MainGameViewModel.SelectedWeapon.Damage - 5, MainGameViewModel.SelectedWeapon.Damage + 6);
                damage = damage + _playerAttackBonus - _monsterDefenseBonus;
                if (damage <= 0) damage = 0;
                ActiveMonster.Monster.Hp -= damage;
                if (MainGameViewModel.ActivePlayerManager.Player.Hp <= 0)
                {
                    MessageList.Insert(0, String.Format("{0} vous frappe pour {1} dégats, vous mourrez.", ActiveMonster.Monster.Name, damage));
                }
                else
                {
                    MessageList.Insert(0, String.Format("{0} vous frappe pour {1} dégats, il vous reste {2} PVs.", ActiveMonster.Monster.Name, damage, MainGameViewModel.ActivePlayerManager.Player.Hp));
                }
                NotifyPropertyChanged("ActivePlayerManager");
                NotifyPropertyChanged("ActiveMonster");
            }
        }
    }
}
