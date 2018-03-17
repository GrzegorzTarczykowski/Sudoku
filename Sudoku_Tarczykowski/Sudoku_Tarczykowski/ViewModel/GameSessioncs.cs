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
using System.Windows.Input;

namespace Sudoku_Tarczykowski.ViewModel
{
    class GameSessioncs : INotifyPropertyChanged
    {
        private Page mainFrame;
        public Board currentBoard;
        private ICommand buttonNewGameClickCommand;
        private ICommand buttonSaveGameClickCommand;
        private ICommand buttonLoadGameClickCommand;
        private ICommand buttonHelpClickCommand;
        private ICommand buttonSolveGameClickCommand;
        private ICommand buttonPrintGameClickCommand;
        public GameSessioncs()
        {
            MainFrame = new PlaygroundPage();
            MainFrame.DataContext = this;
        }
        private void OnAddedField(object o, FieldEventArgs e)
        {
            bool isUniqueNumber = false;
            if (e.ValueField != "")
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
            if (isUniqueNumber)
            {
                ((Field)o).IsCorrect = true;
                ((Field)o).ValueField = e.ValueField;
            }
            else
            {
                MessageBox.Show("Cyfra w kazdym wierszu, kolumnie i kwadracie musi być unikatowa!");
            }
        }
        private void buttonNewGameAction()
        {
            BoardFactory boardFactory = new BoardFactory();
            CurrentBoard = boardFactory.CreateBoard();
            foreach (Field field in CurrentBoard.Fields)
            {
                field.AllFieldAreCreated = true;
                field.AddedValueField += OnAddedField;
            }
        }
        private void buttonSaveGameAction()
        {
            MessageBox.Show("Zapisuje");
        }
        private void buttonLoadGameAction()
        {
            MessageBox.Show("Wczytuje");
        }
        private void buttonHelpAction()
        {
            MessageBox.Show("Pomoc");
        }
        private void buttonSolveGameAction()
        {
            MessageBox.Show("Rozwiązanie");
        }
        private void buttonPrintGameAction()
        {
            MessageBox.Show("Drukuj");
        }
        #region Property
        public ICommand ButtonNewGameClickCommand
        {
            get
            {
                return buttonNewGameClickCommand ?? (buttonNewGameClickCommand = new CommandHandler(() => buttonNewGameAction(), true));
            }
        }
        public ICommand ButtonSaveGameClickCommand
        {
            get
            {
                return buttonSaveGameClickCommand ?? (buttonSaveGameClickCommand = new CommandHandler(() => buttonSaveGameAction(), true));
            }
        }
        public ICommand ButtonLoadGameClickCommand
        {
            get
            {
                return buttonLoadGameClickCommand ?? (buttonLoadGameClickCommand = new CommandHandler(() => buttonLoadGameAction(), true));
            }
        }
        public ICommand ButtonHelpClickCommand
        {
            get
            {
                return buttonHelpClickCommand ?? (buttonHelpClickCommand = new CommandHandler(() => buttonHelpAction(), true));
            }
        }
        public ICommand ButtonSolveGameClickCommand
        {
            get
            {
                return buttonSolveGameClickCommand ?? (buttonSolveGameClickCommand = new CommandHandler(() => buttonSolveGameAction(), true));
            }
        }
        public ICommand ButtonPrintGameClickCommand
        {
            get
            {
                return buttonPrintGameClickCommand ?? (buttonPrintGameClickCommand = new CommandHandler(() => buttonPrintGameAction(), true));
            }
        }
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
        public Board CurrentBoard
        {
            get { return currentBoard; }
            set
            {
                if (value != currentBoard)
                {
                    currentBoard = value;
                    OnPropertyChanged("CurrentBoard");
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
