using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Sudoku_Tarczykowski.View;

namespace Sudoku_Tarczykowski.ViewModel
{
    class GameSessioncs : INotifyPropertyChanged
    {
        private Page mainFrame;
        public GameSessioncs()
        {
            MainFrame = new PlaygroundPage();
            MainFrame.DataContext = this;
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
