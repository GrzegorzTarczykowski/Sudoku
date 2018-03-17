using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using Sudoku_Tarczykowski.ViewModel;

namespace Sudoku_Tarczykowski.Model
{
    class Field : INotifyPropertyChanged
    {
        private string valueField;
        private int row;
        private int column;
        private int square;
        public bool IsCorrect { get; set; }
        public bool AllFieldAreCreated { get; set; }
        public delegate void AddedValueFieldEventArgs(object o, FieldEventArgs e);
        public event AddedValueFieldEventArgs AddedValueField;
        public Field(string valueField, int row, int column, int square)
        {
            AllFieldAreCreated = false;
            IsCorrect = false;
            ValueField = valueField;
            Row = row;
            Column = column;
            Square = square;
        }
        protected virtual void OnAddedField (string value)
        {
            if(AddedValueField != null)
            {
                AddedValueField(this, new FieldEventArgs() { ValueField = value });
            }
        }
        #region Property
        public string ValueField
        {
            get { return valueField; }
            set
            {
                if(AllFieldAreCreated)
                {
                    if (Regex.IsMatch(value, "([1-9])") || value == "")
                    {
                        if (IsCorrect)
                        {
                            if (value != valueField)
                            {
                                valueField = value;
                                OnPropertyChanged("ValueField");
                            }
                            IsCorrect = false;
                        }
                        else
                        {
                            OnAddedField(value);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Uzyj cyfry od 1 do 9");
                    }
                }
                else
                {
                    if (value != valueField)
                    {
                        valueField = value;
                        OnPropertyChanged("ValueField");
                    }
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
