using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.Win32;
using System.Runtime.Intrinsics.X86;

namespace Spreadsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<DataRow> data = new List<DataRow>();
        List<DataRow> formulaData = new List<DataRow>();
        Stack<string [,]> listaStanja = new Stack<string[,]>();
        Stack<string[,]> listaRedo = new Stack<string[,]>();
        public MainWindow()
        {
            InitializeComponent();
            string col = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int j = 0;
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            for (int i = 0; i < 50; i++)
            {
                if (i < 26)
                {
                    dt.Columns.Add(col[i].ToString());
                    dt2.Columns.Add(col[i].ToString());
                }
                else
                {
                    string cc = col[j].ToString() + col[j].ToString();
                    dt.Columns.Add(cc);
                    dt2.Columns.Add(cc);
                    j++;
                }
                dt.Rows.Add();
                dt2.Rows.Add();
                data.Add(dt.Rows[i]);
                formulaData.Add(dt2.Rows[i]);
            }
            dg.ItemsSource = dt.DefaultView;
            
        }

        void WriteFile(List<DataRow> data, string path)
        {
            string[,] s = new string[data.Count, 50];
            for (int i = 0; i < s.GetLength(0); i++)
            {
                for (int j = 0; j < s.GetLength(1); j++)
                {
                    s[i, j] = data[i][j].ToString();
                    if (s[i, j] == "")
                    {
                        File.AppendAllText(path, "/,");
                    }
                    else
                    {
                        File.AppendAllText(path, s[i, j] + "/,");
                    }
                }
                File.AppendAllText(path, "/|/,");
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.AddExtension = true;
            save.Filter = "CSV Files (*.csv)|*.csv";
            if (save.ShowDialog() == true)
            {
                string path = save.FileName;
                File.WriteAllText(path, "");
                WriteFile(data, path);
                WriteFile(formulaData, path);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "CSV Files (*.csv)|*.csv";
            if (open.ShowDialog() == true)
            {
                string path = open.FileName;
                string s1 = File.ReadAllText(path);
                string[] s2 = s1.Split("/,");
                int j = 0;
                int k = 0;
                int l = 0;
                int m = 0;
                for (int i = 0; i < s2.Length - 1; i++)
                {
                    if (j < 50)
                    {
                        if (s2[i].Equals("/|"))
                        {
                            k = 0;
                            j++;
                        }
                        else
                        {
                            data[j][k] = s2[i];
                            k++;
                        }
                    }
                    else
                    {
                        if (s2[i].Equals("/|"))
                        {
                            l = 0;
                            m++;
                        }
                        else
                        {
                            formulaData[m][l] = s2[i];
                            l++;
                        }
                    }
                }

            }
        }

        private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()+ 1).ToString();
        }

        private void dg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditingElement.ToString()[0] != '=')
            {
                formulaData[e.Row.GetIndex()][e.Column.DisplayIndex] = "";
            }
            string [,] m = new string[50, 50];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    m[i, j] = data[i][j].ToString();
                }
            }
            listaStanja.Push(m);

        }

        private void btUndo_Click(object sender, RoutedEventArgs e)
        {
            string[,] sada0 = new string[50,50];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    sada0[i,j] = data[i][j].ToString();
                }
            }

            listaRedo.Push(sada0);
            if (listaStanja.Count != 0)
            {
                string [,] sada = listaStanja.Peek();
                
                listaStanja.Pop();
                for(int i = 0; i < 50; i++)
                {
                    for(int j = 0; j < 50; j++)
                    {
                        data[i][j] = sada[i,j];
                    }
                }
            }
        }

        private void btRedo_Click(object sender, RoutedEventArgs e)
        {
            string[,] sada0 = new string[50, 50];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    sada0[i, j] = data[i][j].ToString();
                }
            }
            listaStanja.Push(sada0);
            if (listaRedo.Count != 0)
            {
                string[,] sada = listaRedo.Peek();
                listaRedo.Pop();

                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        data[i][j] = sada[i, j];
                    }
                }
            }
        }
        int GetColIndex(string cell)
        {
            int colIndex = cell[0] - 65;
            if (cell.Length == 3 || cell.Length == 4)
            {
                if (!int.TryParse(cell[1].ToString(), out int x))
                {
                    colIndex = cell[1] - 65 + 26;
                }
            }
            return colIndex;
        }
        int GetRowIndex(string cell)
        {
            int rowIndex;
            if (cell.Length == 3)
            {
                if (int.TryParse(cell[1].ToString(), out int x))
                {
                    rowIndex = int.Parse(cell.Substring(1)) - 1;
                }
                else
                {
                    rowIndex = int.Parse(cell[2].ToString()) - 1;
                }
            }
            else if (cell.Length == 4)
            {
                rowIndex = int.Parse(cell.Substring(2)) - 1;
            }
            else { rowIndex = int.Parse(cell[1].ToString()) - 1; }
            return rowIndex;
        }
        void CheckFormula(string content, int i, int j)
        {
            if (content[0].Equals('='))
            {
                string formula = content.Split('(', ')')[0];
                string operands = content.Split('(', ')')[1];
                bool isCircular = false;
                if (formula.ToUpper().Equals("=SUM"))
                {
                    string[] cells = operands.Split(',');
                    float sum = 0;
                    foreach (string cell in cells)
                    {
                        if (float.TryParse(cell, out float cellV))
                        {
                            sum += cellV;
                        }
                        else
                        {
                            int rowIndex = 0;
                            int colIndex = 0;
                            colIndex = GetColIndex(cell);
                            rowIndex = GetRowIndex(cell);
                            isCircular = CheckIfCircular(i, j, rowIndex, colIndex);
                            if (colIndex == j && rowIndex == i)
                            {
                                isCircular = true;
                            }
                            if (float.TryParse(data[rowIndex][colIndex].ToString(), out float cellValue))
                            {
                                sum += cellValue;
                            }
                        }
                    }
                    if (isCircular)
                    {
                        data[i][j] = 0;
                    }
                    else
                    {
                        data[i][j] = sum;
                    }
                    formulaData[i][j] = content;
                }
                else if (formula.ToUpper().Equals("=SUMRANGE"))
                {
                    string[] cells = operands.Split(':');
                    float sum = 0;
                    int[,] indexes = new int[2,2];
                    int k = 0;
                    foreach (string cell in cells)
                    {
                        indexes[k, 0] = GetColIndex(cell);
                        indexes[k, 1] = GetRowIndex(cell);
                        isCircular = CheckIfCircular(i, j, indexes[k, 1], indexes[k, 0]);
                        if (indexes[k, 0] == j && indexes[k, 1] == i)
                        {
                            isCircular = true;
                        }
                        k++;
                    }
                    for (int n = indexes[0, 0]; n <= indexes[1, 0]; n++)
                    {
                        for (int m = indexes[0, 1]; m <= indexes[1, 1]; m++)
                        {
                            if (float.TryParse(data[m][n].ToString(), out float cellValue))
                            {
                                sum += cellValue;
                            }
                        }
                    }
                    if (isCircular)
                    {
                        data[i][j] = 0;
                    }
                    else
                    {
                        data[i][j] = sum;
                    }
                    formulaData[i][j] = content;
                }
                else if (formula.ToUpper().Equals("=AVG"))
                {
                    string[] cells = operands.Split(',');
                    float avg = 0;
                    foreach (string cell in cells)
                    {
                        if (float.TryParse(cell, out float cellV))
                        {
                            avg += cellV;
                        }
                        else
                        {
                            int rowIndex = 0;
                            int colIndex = 0;
                            colIndex = GetColIndex(cell);
                            rowIndex = GetRowIndex(cell);
                            isCircular = CheckIfCircular(i, j, rowIndex, colIndex);
                            if (colIndex == j && rowIndex == i)
                            {
                                isCircular = true;
                            }
                            if (float.TryParse(data[rowIndex][colIndex].ToString(), out float cellValue))
                            {
                                avg += cellValue;
                            }
                        }
                    }
                    avg /= cells.Length;
                    if (isCircular)
                    {
                        data[i][j] = 0;
                    }
                    else
                    {
                        data[i][j] = avg;
                    }
                    formulaData[i][j] = content;
                }
                else if (formula.ToUpper().Equals("=MAX"))
                {
                    string[] cells = operands.Split(',');
                    float[] cellValues = new float[cells.Length];
                    int count = 0;
                    foreach (string cell in cells)
                    {
                        if (float.TryParse(cell, out float cellV))
                        {
                            cellValues[count] += cellV;
                        }
                        else
                        {
                            int rowIndex = 0;
                            int colIndex = 0;
                            colIndex = GetColIndex(cell);
                            rowIndex = GetRowIndex(cell);
                            isCircular = CheckIfCircular(i, j, rowIndex, colIndex);
                            if (colIndex == j && rowIndex == i)
                            {
                                isCircular = true;
                            }
                            if (float.TryParse(data[rowIndex][colIndex].ToString(), out float cellValue1))
                            {
                                cellValues[count] = cellValue1;
                            }
                        }
                        count++;
                    }
                    float max = 0;
                    foreach (int cellValue in cellValues)
                    {
                        max = Math.Max(max, cellValue);
                    }
                    if (isCircular)
                    {
                        data[i][j] = 0;
                    }
                    else
                    {
                        data[i][j] = max;
                    }
                    formulaData[i][j] = content;
                }
                else if (formula.ToUpper().Equals("=MIN"))
                {
                    string[] cells = operands.Split(',');
                    float[] cellValues = new float[cells.Length];
                    int count = 0;
                    foreach (string cell in cells)
                    {
                        if (float.TryParse(cell, out float cellV))
                        {
                            cellValues[count] += cellV;
                        }
                        else
                        {
                            int rowIndex = 0;
                            int colIndex = 0;
                            colIndex = GetColIndex(cell);
                            rowIndex = GetRowIndex(cell);
                            isCircular = CheckIfCircular(i, j, rowIndex, colIndex);
                            if (colIndex == j && rowIndex == i)
                            {
                                isCircular = true;
                            }
                            if (float.TryParse(data[rowIndex][colIndex].ToString(), out float cellValue1))
                            {
                                cellValues[count] = cellValue1;
                            }
                        }
                        count++;
                    }
                    float min = cellValues[0];
                    foreach (int cellValue in cellValues)
                    {
                        min = Math.Min(min, cellValue);
                    }
                    if (isCircular)
                    {
                        data[i][j] = 0;
                    }
                    else
                    {
                        data[i][j] = min;
                    }
                    formulaData[i][j] = content;
                }
            }
        }
        bool CheckIfCircular(int i, int j, int x, int y)
        {
            string fContent = formulaData[x][y].ToString();
            if (!(fContent.Equals("")))
            {
                string[] operands = fContent.Split('(', ')')[1].Split(',');
                foreach (string operand in operands)
                {
                    if (GetColIndex(operand) == j && GetRowIndex(operand) == i)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void dg_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            string[,] m = new string[50, 50];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    m[i, j] = data[i][j].ToString();

                    if (!m[i, j].Equals(""))
                    {
                        string content = m[i, j];
                        string fContent = formulaData[i][j].ToString();
                        if (!fContent.Equals(""))
                        {
                            CheckFormula(fContent, i, j);
                        }
                        else
                        {
                            CheckFormula(content, i, j);
                        }
                    }
                }
            }
        }
    }
}
