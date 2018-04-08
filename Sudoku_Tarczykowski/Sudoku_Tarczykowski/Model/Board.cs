using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Sudoku_Tarczykowski.Model
{
    class Board
    {
        public ObservableCollection<Field> Fields { get; set; }
        public Board()
        {
            Fields = new ObservableCollection<Field>();
        }
        public bool ValidatorBoard()
        {
            bool isUniqueNumber = true;
            foreach (Field checkedField in Fields)
            {
                if (Regex.IsMatch(checkedField.ValueField, "([^0-9])"))
                {
                    isUniqueNumber = false;
                    break;
                }
                foreach (Field field in Fields)
                {
                    if (checkedField.ValueField != "0")
                    {
                        if (!(checkedField.ValueField == field.ValueField && checkedField.Row == field.Row && checkedField.Column == field.Column && checkedField.Square == field.Square))
                        {
                            if (checkedField.ValueField == field.ValueField && (checkedField.Row == field.Row || checkedField.Column == field.Column || checkedField.Square == field.Square))
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
                }
                if (!isUniqueNumber)
                {
                    break;
                }
            }
            return isUniqueNumber;
        }
        public bool ShortValidatorBoard(Field currentCheckedField)
        {
            bool isUniqueNumber = true;
            foreach (Field field in Fields)
            {
                if (!(currentCheckedField.ValueField == field.ValueField && currentCheckedField.Row == field.Row && currentCheckedField.Column == field.Column && currentCheckedField.Square == field.Square))
                {
                    if (currentCheckedField.ValueField == field.ValueField && (currentCheckedField.Row == field.Row || currentCheckedField.Column == field.Column || currentCheckedField.Square == field.Square))
                    {
                        isUniqueNumber = false;
                        break;
                    }
                }
            }
            return isUniqueNumber;
        }
    }
}
