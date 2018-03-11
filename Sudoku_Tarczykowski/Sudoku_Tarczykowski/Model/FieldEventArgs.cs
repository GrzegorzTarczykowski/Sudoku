using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Tarczykowski.Model
{
    class FieldEventArgs : EventArgs
    {
        public string ValueField { get; set; }
    }
}
