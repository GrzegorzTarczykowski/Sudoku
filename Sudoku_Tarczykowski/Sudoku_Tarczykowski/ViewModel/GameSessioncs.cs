﻿using System;
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
using System.Threading;
using Microsoft.Win32;
using System.Windows.Documents;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Sudoku_Tarczykowski.ViewModel
{
    class GameSessioncs : INotifyPropertyChanged
    {
        private Page mainFrame;
        public Board currentBoard;
        bool _canSaveAction;
        bool _canHelpAction;
        bool _canSolveAction;
        bool _canPrintAction;
        private ICommand buttonNewGameClickCommand;
        private ICommand buttonSaveGameClickCommand;
        private ICommand buttonLoadGameClickCommand;
        private ICommand buttonHelpClickCommand;
        private ICommand buttonSolveGameClickCommand;
        private ICommand buttonPrintGameClickCommand;
        private ObservableCollection<int> levelOfTheGame;
        private int selectedLevel;
        private int amountOfHelp;
        private string chosenTextBoxToHelp;
        private Thread threadHelp;
        private Thread threadSolver;
        public GameSessioncs()
        {
            MainFrame = new PlaygroundPage();
            MainFrame.DataContext = this;
            _canSaveAction = false;
            _canHelpAction = false;
            _canSolveAction = false;
            _canPrintAction = false;
            levelOfTheGame = new ObservableCollection<int>() { 1, 2, 3 };
            selectedLevel = new int();
            SelectedLevel = 1;
            amountOfHelp = 3;
            threadHelp = new Thread(ThreadMethodHelp);
            threadSolver = new Thread(ThreadMethodSolver);
            AmountOfHelp = 0;
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
        }
        private void RecursivesSolver(int currentFieldToCheck, ref Board tmpCurrentBoard, ref bool isDone, ref Stack<int> stackOfIndexEmptyField)
        {
            for (int i = 1; i < 11; i++)
            {
                if (isDone)
                {
                    break;
                }
                else
                {
                    if (i == 10)
                    {
                        tmpCurrentBoard.Fields[currentFieldToCheck].ValueField = "";
                        stackOfIndexEmptyField.Push(currentFieldToCheck);
                        break;
                    }
                    else
                    {
                        tmpCurrentBoard.Fields[currentFieldToCheck].ValueField = "";
                        tmpCurrentBoard.Fields[currentFieldToCheck].ValueField = i.ToString();
                        if (tmpCurrentBoard.ShortValidatorBoard(tmpCurrentBoard.Fields[currentFieldToCheck]))
                        {
                            if (stackOfIndexEmptyField.Count > 0)
                            {
                                RecursivesSolver(stackOfIndexEmptyField.Pop(), ref tmpCurrentBoard, ref isDone, ref stackOfIndexEmptyField);
                            }
                            else
                            {
                                isDone = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public void ThreadMethodHelp()
        {
            MessageBox.Show("Kliknij dwukrotnie na pole, ktore ma byc odkryte");
            Thread.CurrentThread.Suspend();
            bool isDone = false;
            Stack<int> stackOfIndexEmptyField = new Stack<int>();
            List<string> listOfValueFromCurrentBoard = new List<string>();
            for (int i = CurrentBoard.Fields.Count - 1; i >= 0; i--)
            {
                if (CurrentBoard.Fields[i].ValueField == "")
                {
                    stackOfIndexEmptyField.Push(i);
                }
            }
            foreach (Field field in CurrentBoard.Fields)
            {
                listOfValueFromCurrentBoard.Add(field.ValueField);
            }
            BoardFactory boardFactory = new BoardFactory();
            Board tmpCurrentBoard = boardFactory.CreateBoardBasedOnLoading(listOfValueFromCurrentBoard);
            if (stackOfIndexEmptyField.Count != 0)
            {
                RecursivesSolver(stackOfIndexEmptyField.Pop(), ref tmpCurrentBoard, ref isDone, ref stackOfIndexEmptyField);
                if (isDone)
                {
                    CurrentBoard.Fields[Convert.ToInt32(ChosenTextBoxToHelp)].ValueField = tmpCurrentBoard.Fields[Convert.ToInt32(ChosenTextBoxToHelp)].ValueField;
                }
                else
                {
                    MessageBox.Show("Nie można znalesc rozwiazania");
                }
                AmountOfHelp--;
                if (AmountOfHelp == 0)
                {
                    _canHelpAction = false;
                    ButtonHelpClickCommand = null;
                }
                isDone = false;
                foreach (Field field in CurrentBoard.Fields)
                {
                    if (field.ValueField == "")
                    {
                        isDone = false;
                        break;
                    }
                    else
                    {
                        isDone = true;
                    }
                }
                if (isDone)
                {
                    _canHelpAction = false;
                    ButtonHelpClickCommand = null;
                    _canSolveAction = false;
                    ButtonSolveGameClickCommand = null;
                }
            }

        }
        public void ThreadMethodSolver()
        {
            bool isDone = false;
            Stack<int> stackOfIndexEmptyField = new Stack<int>();
            List<string> listOfValueFromCurrentBoard = new List<string>();
            for (int i = CurrentBoard.Fields.Count - 1; i >= 0; i--)
            {
                if (CurrentBoard.Fields[i].ValueField == "")
                {
                    stackOfIndexEmptyField.Push(i);
                }
            }
            foreach (Field field in CurrentBoard.Fields)
            {
                listOfValueFromCurrentBoard.Add(field.ValueField);
            }
            BoardFactory boardFactory = new BoardFactory();
            Board tmpCurrentBoard = boardFactory.CreateBoardBasedOnLoading(listOfValueFromCurrentBoard);
            if (stackOfIndexEmptyField.Count != 0)
            {
                RecursivesSolver(stackOfIndexEmptyField.Pop(), ref tmpCurrentBoard, ref isDone, ref stackOfIndexEmptyField);
                if (isDone)
                {
                    CurrentBoard = tmpCurrentBoard;
                    foreach (Field field in CurrentBoard.Fields)
                    {
                        field.AllFieldAreCreated = true;
                        field.AddedValueField += OnAddedField;
                    }
                    _canHelpAction = false;
                    ButtonHelpClickCommand = null;
                    _canSolveAction = false;
                    ButtonSolveGameClickCommand = null;
                }
                else
                {
                    MessageBox.Show("Nie można znalesc rozwiazania");
                }
            }
        }
        private void ButtonNewGameAction()
        {
            BoardFactory boardFactory = new BoardFactory();
            CurrentBoard = boardFactory.CreateBoard();
            _canSaveAction = true;
            ButtonSaveGameClickCommand = null;
            _canHelpAction = true;
            ButtonHelpClickCommand = null;
            _canSolveAction = true;
            AmountOfHelp = 3;
            ButtonSolveGameClickCommand = null;
            _canPrintAction = true;
            ButtonPrintGameClickCommand = null;
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
            List<int> listOfRandomEmpltyFieldIndex = new List<int>();
            Random random = new Random();
            listOfRandomEmpltyFieldIndex.Add(random.Next(0, 80));
            while (listOfRandomEmpltyFieldIndex.Count < howManyFieldsToDoEmptyYet)
            {
                int poczatek = 0;
                int koniec = 80;
                List<int> _listOfRandomEmpltyFieldIndex = new List<int>();
                foreach (int randomNumber in listOfRandomEmpltyFieldIndex)
                {
                    if (listOfRandomEmpltyFieldIndex.Count + _listOfRandomEmpltyFieldIndex.Count < howManyFieldsToDoEmptyYet)
                    {
                        if (poczatek < randomNumber - 1)
                        {
                            _listOfRandomEmpltyFieldIndex.Add(random.Next(poczatek + 1, randomNumber - 1));
                        }
                        poczatek = randomNumber;
                    }
                    else
                    {
                        break;
                    }
                }
                if (listOfRandomEmpltyFieldIndex.Count + _listOfRandomEmpltyFieldIndex.Count < howManyFieldsToDoEmptyYet)
                {
                    if (poczatek < koniec)
                    {
                        _listOfRandomEmpltyFieldIndex.Add(random.Next(poczatek + 1, koniec));
                    }
                }
                foreach (int randomNumber in _listOfRandomEmpltyFieldIndex)
                {
                    listOfRandomEmpltyFieldIndex.Add(randomNumber);
                }
                listOfRandomEmpltyFieldIndex.Sort();
            }
            foreach (int randomNumber in listOfRandomEmpltyFieldIndex)
            {
                CurrentBoard.Fields[randomNumber].ValueField = "";
            }
            if (threadHelp.ThreadState == ThreadState.Suspended)
            {
                threadHelp.Resume();
                threadHelp.Abort();
            }
        }
        private void ButtonSaveGameAction()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt | *.txt";
            sfd.FileName = "Sudoku.txt";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
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
            if (threadHelp.ThreadState == ThreadState.Suspended)
            {
                threadHelp.Resume();
                threadHelp.Abort();
            }
        }
        private void ButtonLoadGameAction()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt | *.txt";
            ofd.FileName = "Sudoku.txt";
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                StreamReader sr = new StreamReader(ofd.FileName);
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
                        foreach (Field field in temporaryBoard.Fields)
                        {
                            if (field.ValueField == "0")
                            {
                                field.ValueField = "";
                            }
                        }
                        CurrentBoard = temporaryBoard;
                        _canSaveAction = true;
                        ButtonSaveGameClickCommand = null;
                        _canHelpAction = false;
                        ButtonHelpClickCommand = null;
                        _canSolveAction = true;
                        ButtonSolveGameClickCommand = null;
                        _canPrintAction = true;
                        ButtonPrintGameClickCommand = null;
                        AmountOfHelp = 0;
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
            if (threadHelp.ThreadState == ThreadState.Suspended)
            {
                threadHelp.Resume();
                threadHelp.Abort();
            }
        }
        private void ButtonHelpAction()
        {
            if (!threadHelp.IsAlive)
            {
                threadHelp = new Thread(ThreadMethodHelp);
                threadHelp.Start();
            }
            else
            {
                MessageBox.Show("Kliknij dwukrotnie na pole, ktore ma byc odkryte");
            }
        }
        private void ButtonSolveGameAction()
        {
            if (!threadSolver.IsAlive)
            {
                threadSolver = new Thread(ThreadMethodSolver);
                threadSolver.Start();
            }
            else
            {
                MessageBox.Show("Poczekaj na rozwiazanie");
            }
            if (threadHelp.ThreadState == ThreadState.Suspended)
            {
                threadHelp.Resume();
                threadHelp.Abort();
            }
        }
        private void ButtonPrintGameAction()
        {
            PdfPTable pdfTable = new PdfPTable(9);
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTable.DefaultCell.FixedHeight = 45;
            foreach (Field field in CurrentBoard.Fields)
            {
                if (field.ValueField != "")
                {
                    pdfTable.AddCell(field.ValueField);
                }
                else
                {
                    pdfTable.AddCell(" ");
                }
            }
            Document doc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            PdfWriter.GetInstance(doc, new FileStream("S.pdf", FileMode.Create));
            doc.Open();
            doc.Add(pdfTable);
            doc.Close();
            System.Diagnostics.Process.Start(@"S.pdf");
            if (threadHelp.ThreadState == ThreadState.Suspended)
            {
                threadHelp.Resume();
                threadHelp.Abort();
            }
        }
        #region Property
        public ICommand ButtonNewGameClickCommand
        {
            get
            {
                return buttonNewGameClickCommand ?? (buttonNewGameClickCommand = new CommandHandler(() => ButtonNewGameAction(), true));
            }
        }
        public ICommand ButtonSaveGameClickCommand
        {
            get
            {
                return buttonSaveGameClickCommand ?? (buttonSaveGameClickCommand = new CommandHandler(() => ButtonSaveGameAction(), _canSaveAction));
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
                return buttonLoadGameClickCommand ?? (buttonLoadGameClickCommand = new CommandHandler(() => ButtonLoadGameAction(), true));
            }
        }
        public ICommand ButtonHelpClickCommand
        {
            get
            {
                return buttonHelpClickCommand ?? (buttonHelpClickCommand = new CommandHandler(() => ButtonHelpAction(), _canHelpAction));
            }
            set
            {
                if (value != buttonHelpClickCommand)
                {
                    buttonHelpClickCommand = value;
                    OnPropertyChanged("ButtonHelpClickCommand");
                }
            }
        }
        public ICommand ButtonSolveGameClickCommand
        {
            get
            {
                return buttonSolveGameClickCommand ?? (buttonSolveGameClickCommand = new CommandHandler(() => ButtonSolveGameAction(), _canSolveAction));
            }
            set
            {
                if (value != buttonSolveGameClickCommand)
                {
                    buttonSolveGameClickCommand = value;
                    OnPropertyChanged("ButtonSolveGameClickCommand");
                }
            }
        }
        public ICommand ButtonPrintGameClickCommand
        {
            get
            {
                return buttonPrintGameClickCommand ?? (buttonPrintGameClickCommand = new CommandHandler(() => ButtonPrintGameAction(), _canPrintAction));
            }
            set
            {
                if (value != buttonPrintGameClickCommand)
                {
                    buttonPrintGameClickCommand = value;
                    OnPropertyChanged("ButtonPrintGameClickCommand");
                }
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
        public int AmountOfHelp
        {
            get { return amountOfHelp; }
            set
            {
                amountOfHelp = value;
                OnPropertyChanged("AmountOfHelp");
            }
        }
        public string ChosenTextBoxToHelp
        {
            get { return chosenTextBoxToHelp; }
            set
            {
                if (threadHelp.ThreadState == ThreadState.Suspended)
                    threadHelp.Resume();
                chosenTextBoxToHelp = value;
                OnPropertyChanged("ChosenTextBoxToHelp");
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
