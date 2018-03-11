using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Sudoku_Tarczykowski.Model
{
    class Board
    {
        public ObservableCollection<Field> Fields { get; set; }
        public Board()
        {
            Fields = new ObservableCollection<Field>();
        }
    }
}
