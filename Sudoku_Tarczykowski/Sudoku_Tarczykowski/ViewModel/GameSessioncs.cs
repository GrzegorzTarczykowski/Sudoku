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
using System.Collections.ObjectModel;
using System.IO;

namespace Sudoku_Tarczykowski.ViewModel
{
    class GameSessioncs : INotifyPropertyChanged
    {
        private Page mainFrame;
        public Board currentBoard;
        bool _canSaveAction;
        private ICommand buttonNewGameClickCommand;
        private ICommand buttonSaveGameClickCommand;
        private ICommand buttonLoadGameClickCommand;
        private ICommand buttonHelpClickCommand;
        private ICommand buttonSolveGameClickCommand;
        private ICommand buttonPrintGameClickCommand;
        private ObservableCollection<int> levelOfTheGame;
        private int selectedLevel;
        public GameSessioncs()
        {
            MainFrame = new PlaygroundPage();
            MainFrame.DataContext = this;
            _canSaveAction = false;
            levelOfTheGame = new ObservableCollection<int>() { 1, 2, 3 };
            selectedLevel = new int();
            SelectedLevel = 1;
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
        private void RecursivesGenerateRandomEmptyField(int minValue, int maxValue, ref int howManyFieldsToDoEmptyYet)
        {
            Random random = new Random();
            int currentRandom = random.Next(minValue, maxValue);
            CurrentBoard.Fields[currentRandom].ValueField = "";
            howManyFieldsToDoEmptyYet--;
            if (howManyFieldsToDoEmptyYet > 1)
            {
                if (currentRandom > 0 && minValue < currentRandom - 1)
                {
                    RecursivesGenerateRandomEmptyField(minValue, currentRandom - 1, ref howManyFieldsToDoEmptyYet);
                }
                if (currentRandom < 80 && currentRandom + 1 < maxValue)
                {
                    RecursivesGenerateRandomEmptyField(currentRandom + 1, maxValue, ref howManyFieldsToDoEmptyYet);
                }
            }
            else if (howManyFieldsToDoEmptyYet > 0)
            {
                if (currentRandom > 0 && minValue < currentRandom - 1)
                {
                    RecursivesGenerateRandomEmptyField(minValue, currentRandom - 1, ref howManyFieldsToDoEmptyYet);
                }
            }
        }
        private void buttonNewGameAction()
        {
            BoardFactory boardFactory = new BoardFactory();
            CurrentBoard = boardFactory.CreateBoard();
            _canSaveAction = true;
            ButtonSaveGameClickCommand = null;
            foreach (Field field in CurrentBoard.Fields)
            {
                field.AllFieldAreCreated = true;
                field.AddedValueField += OnAddedField;
            }
            int howManyFieldsToDoEmptyYet = 0;
            switch (SelectedLevel)
            {
                case 1:
                    howManyFieldsToDoEmptyYet = 30;
                    break;
                case 2:
                    howManyFieldsToDoEmptyYet = 45;
                    break;
                case 3:
                    howManyFieldsToDoEmptyYet = 60;
                    break;
            }
            RecursivesGenerateRandomEmptyField(0, 80, ref howManyFieldsToDoEmptyYet);
        }
        private void buttonSaveGameAction()
        {
            MessageBox.Show("Zapisuje");
            StreamWriter sw = new StreamWriter("Sudoku.txt");
            string tableToSave = "";
            foreach (Field field in CurrentBoard.Fields)
            {
                if (field.ValueField == "")
                {
                    tableToSave += "0" + " ";
                }
                else
                {
                    tableToSave += field.ValueField + " ";
                }
            }
            sw.WriteLine(tableToSave);
            sw.Close();
        }
        private void buttonLoadGameAction()
        {
            MessageBox.Show("Wczytuje");
            StreamReader sr = new StreamReader("Sudoku.txt");
            string tableFromLoad = "";
            tableFromLoad = sr.ReadLine();
            sr.Close();
            if (tableFromLoad.Length == 162)
            {
                List<string> listOfValueFields = new List<string>();
                for (int i = 0; i < 161; i = i + 2)
                {
                    listOfValueFields.Add(tableFromLoad[i].ToString());
                }
                BoardFactory boardFactory = new BoardFactory();
                Board temporaryBoard = boardFactory.CreateBoardBasedOnLoading(listOfValueFields);
                if (temporaryBoard.ValidatorBoard())
                {
                    CurrentBoard = temporaryBoard;
                    _canSaveAction = true;
                    ButtonSaveGameClickCommand = null;
                    foreach (Field field in CurrentBoard.Fields)
                    {
                        field.AllFieldAreCreated = true;
                        field.AddedValueField += OnAddedField;
                    }
                }
                else
                {
                    MessageBox.Show("Nieprawidlowy ciag znakow w pliku Sudoku.txt");
                }
            }
            else
            {
                MessageBox.Show("Ciag znakow w pliku Sudoku.txt musi posiadac 162 znaki");
            }
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
                return buttonSaveGameClickCommand ?? (buttonSaveGameClickCommand = new CommandHandler(() => buttonSaveGameAction(), _canSaveAction));
            }
            set
            {
                if (value != buttonSaveGameClickCommand)
                {
                    buttonSaveGameClickCommand = value;
                    OnPropertyChanged("ButtonSaveGameClickCommand");
                }
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
        public ObservableCollection<int> LevelOfTheGame
        {
            get { return levelOfTheGame; }
            set
            {
                levelOfTheGame = value;
                OnPropertyChanged("LevelOfTheGame");
            }
        }
        public int SelectedLevel
        {
            get { return selectedLevel; }
            set
            {
                selectedLevel = value;
                OnPropertyChanged("SelectedLevel");
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
