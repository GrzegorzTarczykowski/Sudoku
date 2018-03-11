using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Sudoku_Tarczykowski.View;
using Sudoku_Tarczykowski.Model;
using Sudoku_Tarczykowski.Factories;
using System.Windows;

namespace Sudoku_Tarczykowski.ViewModel
{
    class GameSessioncs : INotifyPropertyChanged
    {
        private Page mainFrame;
        public Board CurrentBoard { get; set; }
        public GameSessioncs()
        {
            MainFrame = new PlaygroundPage();
            MainFrame.DataContext = this;
            BoardFactory boardFactory = new BoardFactory();
            CurrentBoard = boardFactory.CreateBoard();
            foreach (Field field in CurrentBoard.Fields)
            {
                field.AllFieldAreCreated = true;
                field.AddedValueField += OnAddedField;
            }
        }
        private void OnAddedField(object o, FieldEventArgs e)
        {
            bool isUniqueNumber = false;
            if(e.ValueField != "")
            {
                foreach (Field field in CurrentBoard.Fields)
                {
                    if (e.ValueField == field.ValueField && (((Field)o).Row == field.Row || ((Field)o).Column == field.Column || ((Field)o).Square == field.Square))
                    {
                        isUniqueNumber = false;
                        break;
                    }
                    else
                    {
                        isUniqueNumber = true;
                    }
                }
            }
            else
            {
                isUniqueNumber = true;
            }
            if(isUniqueNumber)
            {
                ((Field)o).IsCorrect = true;
                ((Field)o).ValueField = e.ValueField;
            }
        }
        #region Property
        public Page MainFrame
        {
            get { return mainFrame; }
            set
            {
                if (value != mainFrame)
                {
                    mainFrame = value;
                    OnPropertyChanged("MainFrame");
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
