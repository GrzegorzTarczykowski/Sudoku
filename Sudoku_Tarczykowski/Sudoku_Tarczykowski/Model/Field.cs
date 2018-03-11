using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Tarczykowski.Model
{
    class Field : INotifyPropertyChanged
    {
        private int valueField;
        private int row;
        private int column;
        private int square;
        public Field(int valueField, int row, int column, int square)
        {
            ValueField = valueField;
            Row = row;
            Column = column;
            Square = square;
        }
        #region Property
        public int ValueField
        {
            get { return valueField; }
            set
            {
                if (value != valueField)
                {
                    valueField = value;
                    OnPropertyChanged("ValueField");
                }
            }
        }
        public int Row
        {
            get { return row; }
            set
            {
                if (value != row)
                {
                    row = value;
                    OnPropertyChanged("Row");
                }
            }
        }
        public int Column
        {
            get { return column; }
            set
            {
                if (value != column)
                {
                    column = value;
                    OnPropertyChanged("Column");
                }
            }
        }
        public int Square
        {
            get { return square; }
            set
            {
                if (value != square)
                {
                    square = value;
                    OnPropertyChanged("Square");
                }
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
