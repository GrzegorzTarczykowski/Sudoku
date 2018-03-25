using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku_Tarczykowski.Model;

namespace Sudoku_Tarczykowski.Factories
{
    internal class BoardFactory
    {
        //Tablice 
        int[][] tableWithRows = new[] { new int [] {1, 2, 3, 4, 5, 6, 7, 8, 9 },
                                        new int [] {4 ,5, 6, 7, 8, 9, 1, 2, 3 },
                                        new int [] {7 ,8, 9, 1, 2, 3, 4, 5, 6 },
                                        new int [] {2, 3, 4, 5, 6, 7, 8, 9, 1 },
                                        new int [] {5, 6, 7, 8, 9, 1, 2, 3, 4 },
                                        new int [] {8, 9, 1, 2, 3, 4, 5, 6, 7 },
                                        new int [] {3, 4 ,5, 6, 7, 8, 9, 1, 2 },
                                        new int [] {6, 7, 8, 9, 1, 2, 3, 4, 5 },
                                        new int [] {9, 1, 2, 3, 4, 5, 6, 7, 8 }};
        int[][] tableWithColumns = new[] { new int [] {1, 4, 7, 2, 5, 8, 3, 6, 9 },
                                           new int [] {2, 5, 8, 3, 6, 9, 4, 7, 1 },
                                           new int [] {3, 6, 9, 4, 7, 1, 5, 8, 2 },
                                           new int [] {4, 7, 1, 5, 8, 2, 6, 9, 3 },
                                           new int [] {5, 8, 2, 6, 9, 3, 7, 1, 4 },
                                           new int [] {6, 9, 3, 7, 1, 4, 8, 2, 5 },
                                           new int [] {7, 1, 4, 8, 2, 5, 9, 3, 6 },
                                           new int [] {8, 2, 5, 9, 3, 6, 1, 4, 7 },
                                           new int [] {9, 3, 6, 1, 4, 7, 2, 5, 8 }};
        internal void refreshTableWithRows()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tableWithRows[i][j] = tableWithColumns[j][i];
                }
            }
        }
        internal void refreshTableWithColumns()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tableWithColumns[i][j] = tableWithRows[j][i];
                }
            }
        }
        internal void mixingNumbers()
        {
            Random random = new Random();
            // Pierwszy wiersz z pierwsza kolmumna
            if(random.Next(10) < 5)
            {
                for (int i = 0; i < 9; i++)
                {
                    var tmp = tableWithRows[i];
                    tableWithRows[i] = tableWithColumns[i];
                    tableWithColumns[i] = tmp;
                }
            }
            // Pierwszy wiersz z ostatnia kolumna
            if(random.Next(10) < 5)
            {
                for (int i = 0; i < 9; i++)
                {
                    var tmp = tableWithRows[i];
                    tableWithRows[i] = tableWithColumns[8 - i];
                    tableWithColumns[8 - i] = tmp;
                }
            }
            // Pierwszy poziom z drugim poziomem
            if(random.Next(10) < 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    var tmp = tableWithRows[i];
                    tableWithRows[i] = tableWithRows[i + 3];
                    tableWithRows[i + 3] = tmp;
                }
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Drugi poziom z trzecim poziomem
            if(random.Next(10) < 5)
            {
                for (int i = 3; i < 6; i++)
                {
                    var tmp = tableWithRows[i];
                    tableWithRows[i] = tableWithRows[i + 3];
                    tableWithRows[i + 3] = tmp;
                }
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Pierwszy poziom z trzecim poziomem
            if(random.Next(10) < 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    var tmp = tableWithRows[i];
                    tableWithRows[i] = tableWithRows[i + 6];
                    tableWithRows[i + 6] = tmp;
                }
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Pierwszy pion z drugim pionem
            if(random.Next(10) < 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    var tmp = tableWithColumns[i];
                    tableWithColumns[i] = tableWithColumns[i + 3];
                    tableWithColumns[i + 3] = tmp;
                }
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Drugi pion z trzecim pionem
            if(random.Next(10) < 5)
            {
                for (int i = 3; i < 6; i++)
                {
                    var tmp = tableWithColumns[i];
                    tableWithColumns[i] = tableWithColumns[i + 3];
                    tableWithColumns[i + 3] = tmp;
                }
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Pierwszy pion z trzecim pionem 
            if(random.Next(10) < 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    var tmp = tableWithColumns[i];
                    tableWithColumns[i] = tableWithColumns[i + 6];
                    tableWithColumns[i + 6] = tmp;
                }
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Pierwszy wiersz z drugim wierszem
            if(random.Next(10) < 5)
            {
                var tmp1 = tableWithRows[0];
                tableWithRows[0] = tableWithRows[1];
                tableWithRows[1] = tmp1;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Drugi wiersz z trzecim wierszem
            if (random.Next(10) < 5)
            {
                var tmp2 = tableWithRows[1];
                tableWithRows[1] = tableWithRows[2];
                tableWithRows[2] = tmp2;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Pierwszy wiersz z trzecim wierszem
            if (random.Next(10) < 5)
            {
                var tmp3 = tableWithRows[0];
                tableWithRows[0] = tableWithRows[2];
                tableWithRows[2] = tmp3;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Czwarty wiersz z piątym wierszem
            if (random.Next(10) < 5)
            {
                var tmp4 = tableWithRows[3];
                tableWithRows[3] = tableWithRows[4];
                tableWithRows[4] = tmp4;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }            // Piąty wiersz z szóstym wierszem
            if (random.Next(10) < 5)
            {
                var tmp5 = tableWithRows[4];
                tableWithRows[4] = tableWithRows[5];
                tableWithRows[5] = tmp5;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Czwarty wiersz z szostym wierszem
            if (random.Next(10) < 5)
            {
                var tmp6 = tableWithRows[3];
                tableWithRows[3] = tableWithRows[5];
                tableWithRows[5] = tmp6;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Siódmy wiersz z ósmym wierszem
            if (random.Next(10) < 5)
            {
                var tmp7 = tableWithRows[6];
                tableWithRows[6] = tableWithRows[7];
                tableWithRows[7] = tmp7;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Ósmy wiersz z dziewiątym wierszem
            if (random.Next(10) < 5)
            {
                var tmp8 = tableWithRows[7];
                tableWithRows[7] = tableWithRows[8];
                tableWithRows[8] = tmp8;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Siódmy wiersz z dziewiątym wierszem
            if (random.Next(10) < 5)
            {
                var tmp9 = tableWithRows[6];
                tableWithRows[6] = tableWithRows[8];
                tableWithRows[8] = tmp9;
                //*********Aktualizacja tKolumn
                refreshTableWithColumns();
            }
            // Pierwszy kolumna z drugim kolumna
            if (random.Next(10) < 5)
            {
                var tmp10 = tableWithColumns[0];
                tableWithColumns[0] = tableWithColumns[1];
                tableWithColumns[1] = tmp10;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Drugi kolumna z trzecim kolumna
            if (random.Next(10) < 5)
            {
                var tmp11 = tableWithColumns[1];
                tableWithColumns[1] = tableWithColumns[2];
                tableWithColumns[2] = tmp11;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Pierwszy kolumna z trzecim kolumna
            if (random.Next(10) < 5)
            {
                var tmp12 = tableWithColumns[0];
                tableWithColumns[0] = tableWithColumns[2];
                tableWithColumns[2] = tmp12;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Czwarty kolumna z piątym kolumna
            if (random.Next(10) < 5)
            {
                var tmp13 = tableWithColumns[3];
                tableWithColumns[3] = tableWithColumns[4];
                tableWithColumns[4] = tmp13;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Piąty kolumna z szóstym kolumna
            if (random.Next(10) < 5)
            {
                var tmp14 = tableWithColumns[4];
                tableWithColumns[4] = tableWithColumns[5];
                tableWithColumns[5] = tmp14;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Czwarty kolumna z szostym kolumna
            if (random.Next(10) < 5)
            {
                var tmp15 = tableWithColumns[3];
                tableWithColumns[3] = tableWithColumns[5];
                tableWithColumns[5] = tmp15;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Siódmy kolumna z ósmym kolumna
            if (random.Next(10) < 5)
            {
                var tmp16 = tableWithColumns[6];
                tableWithColumns[6] = tableWithColumns[7];
                tableWithColumns[7] = tmp16;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Ósmy kolumna z dziewiątym kolumna
            if (random.Next(10) < 5)
            {
                var tmp17 = tableWithColumns[7];
                tableWithColumns[7] = tableWithColumns[8];
                tableWithColumns[8] = tmp17;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
            // Siódmy kolumna z dziewiątym kolumna
            if (random.Next(10) < 5)
            {
                var tmp18 = tableWithColumns[6];
                tableWithColumns[6] = tableWithColumns[8];
                tableWithColumns[8] = tmp18;
                //*********Aktualizacja tWierszy
                refreshTableWithRows();
            }
        }
        internal Board CreateBoard()
        {
            mixingNumbers();
            Board newBoard = new Board();
            int rowID = 0;
            int columnID = 0;
            int squareID = 0;
            foreach (int[] row in tableWithRows)
            {
                foreach(int field in row)
                {
                    if (rowID < 3 && columnID < 3)
                    {
                        squareID = 0;
                    }
                    else if (rowID < 3 && columnID < 6)
                    {
                        squareID = 1;
                    }
                    else if (rowID < 3 && columnID < 9)
                    {
                        squareID = 2;
                    }
                    else if (rowID < 6 && columnID < 3)
                    {
                        squareID = 3;
                    }
                    else if (rowID < 6 && columnID < 6)
                    {
                        squareID = 4;
                    }
                    else if (rowID < 6 && columnID < 9)
                    {
                        squareID = 5;
                    }
                    else if (rowID < 9 && columnID < 3)
                    {
                        squareID = 6;
                    }
                    else if (rowID < 9 && columnID < 6)
                    {
                        squareID = 7;
                    }
                    else if (rowID < 9 && columnID < 9)
                    {
                        squareID = 8;
                    }
                    newBoard.Fields.Add(new Field(field.ToString(), rowID, columnID, squareID));
                    if(columnID == 8)
                    {
                        rowID++;
                        columnID = 0;
                    }
                    else
                    {
                        columnID++;
                    }
                }
            }
            return newBoard;
        }
        internal Board CreateBoardBasedOnLoading(List<string> listOfValueFields)
        {
            Board newBoard = new Board();
            int rowID = 0;
            int columnID = 0;
            int squareID = 0;
            foreach (string valueField in listOfValueFields)
            {
                if (rowID < 3 && columnID < 3)
                {
                    squareID = 0;
                }
                else if (rowID < 3 && columnID < 6)
                {
                    squareID = 1;
                }
                else if (rowID < 3 && columnID < 9)
                {
                    squareID = 2;
                }
                else if (rowID < 6 && columnID < 3)
                {
                    squareID = 3;
                }
                else if (rowID < 6 && columnID < 6)
                {
                    squareID = 4;
                }
                else if (rowID < 6 && columnID < 9)
                {
                    squareID = 5;
                }
                else if (rowID < 9 && columnID < 3)
                {
                    squareID = 6;
                }
                else if (rowID < 9 && columnID < 6)
                {
                    squareID = 7;
                }
                else if (rowID < 9 && columnID < 9)
                {
                    squareID = 8;
                }
                newBoard.Fields.Add(new Field(valueField, rowID, columnID, squareID));
                if (columnID == 8)
                {
                    rowID++;
                    columnID = 0;
                }
                else
                {
                    columnID++;
                }
            }
            return newBoard;
        }
    }
}
