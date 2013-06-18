using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using _2NET_Dal;
using _2NET_Dal.Model;
using _2NET_Gui.Extensions;
using _2NET_Gui.Helpers;
using _2NET_Gui.Managers;
using _2NET_Gui.Views;

namespace _2NET_Gui.ViewModels
{
    public class PlayerSelectionViewModel : ViewModelBase
    {
        private Player _player;
        private Player _selectedPlayer;
        private ObservableCollection<Player> _players;
        private String _newPlayerName;
        private ICommand _newPlayerCommand;
        private ICommand _loadPlayerCommand;
        private ICommand _deletePlayerCommand;

        public Player Player
        {
            get { return _player; }
            set { 
                _player = value;
                NotifyPropertyChanged("Player");
            }
        }

        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                NotifyPropertyChanged("SelectedPlayer");
            }
        }

        public ObservableCollection<Player> Players
        {
            get { return _players; }
            set { 
                _players = value; 
                NotifyPropertyChanged("Players");
            }
        }

        public String NewPlayerName
        {
            get { return _newPlayerName; }
            set
            {
                _newPlayerName = value;
                NotifyPropertyChanged("NewPlayerName");
            }
        }

        public PlayerSelectionViewModel()
        {
            Player = new Player();
            Players = new ObservableCollection<Player>();
            using (var db = new Project2NetContext())
            {
                Players = (from player in db.Players select player).ToObservableCollection();
            }
        }


        public ICommand NewPlayerCommand
        {
            get
            {
                if (_newPlayerCommand == null)
                {
                    _newPlayerCommand = new CommandHelper(param => this.CreateNewPlayer(),
                        null);
                }
                return _newPlayerCommand;
            }
        }

        public ICommand LoadPlayerCommand
        {
            get
            {
                if (_loadPlayerCommand == null)
                {
                    _loadPlayerCommand = new CommandHelper(param => this.LoadPlayer(),
                        CanLoadPlayer);
                }
                return _loadPlayerCommand;
            }
        }
        public bool CanLoadPlayer(object o)
        {
            if (SelectedPlayer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ICommand DeletePlayerCommand
        {
            get
            {
                if (_deletePlayerCommand == null)
                {
                    _deletePlayerCommand = new CommandHelper(param => this.DeletePlayer(),
                        CanDeletePlayer);
                }
                return _deletePlayerCommand;
            }
        }
        public bool CanDeletePlayer(object o)
        {
            if (SelectedPlayer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DeletePlayer()
        {
            new PlayerManager(SelectedPlayer.Id).Delete();
            Players.Remove(SelectedPlayer);
            SelectedPlayer = null;
        }

        public void LoadPlayer()
        {
            if (SelectedPlayer == null) return;
            MainWindow.SelectedPlayerManager = new PlayerManager(SelectedPlayer.Id);
            new LoadingPage();
        }

        public void CreateNewPlayer()
        {
            if (string.IsNullOrEmpty(NewPlayerName)) return;
            MainWindow.SelectedPlayerManager = new PlayerManager(NewPlayerName);
            new LoadingPage();
        }
    }
}
