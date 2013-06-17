using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private ObservableCollection<Cell> _grid = new ObservableCollection<Cell>();
        private ICommand _moveTo;
        private ICommand _useItem;
        private ICommand _exit;

        public MainGameViewModel()
        {
           /* ObservableCollection<Cell> tempGrid = new ObservableCollection<Cell>();
            using (var db = new Project2NetContext())
            {
                tempGrid = (from cell in db.Cells
                            where (cell.PosX >= _player.CurrentCell.PosX - 2 && cell.PosX <= _player.CurrentCell.PosX + 2)
                             && (cell.PosY >= _player.CurrentCell.PosY-2 && cell.PosY <= _player.CurrentCell.PosY+2)
                         select cell).ToObservableCollection();
            }
            foreach (var cell in tempGrid)
            {
                _grid.Add(cell);
            }*/
            for (int i = 0; i <= 25; i++)
            {
                _grid.Add(new CellManager(i,i).Cell);
            }
        }

        public ObservableCollection<Cell> Grid
        {
            get { return _grid; }
        } 


    }
}
