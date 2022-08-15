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

namespace Spreadsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataTable dt = new DataTable();
        public ObservableCollection<DataRow> data = new ObservableCollection<DataRow>();
        public Stack<string [,]> listaStanja = new Stack<string[,]>();
        public Stack<string[,]> listaRedo = new Stack<string[,]>();
        public MainWindow()
        {
            InitializeComponent();
            string col = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int j = 0;
            for (int i = 0; i < 50; i++)
            {
                if (i < 26)
                {
                    dt.Columns.Add(col[i].ToString());
                }
                else
                {
                    string cc = col[j].ToString() + col[j].ToString();
                    dt.Columns.Add(cc);
                    j++;
                }
                dt.Rows.Add();
                data.Add(dt.Rows[i]);
            }
            dg.ItemsSource = dt.DefaultView;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string path = @"C:\Users\Marko\Desktop\sheet.txt";
            File.WriteAllText(path, "");
            string[,] s = new string[data.Count, 50];
            for (int i = 0; i < s.GetLength(0); i++)
            {
                for (int j = 0; j < s.GetLength(1); j++)
                {
                    s[i, j] = data[i][j].ToString();
                    if (s[i, j] == "")
                    {
                        File.AppendAllText(path, "[");
                    }
                    else
                    {
                        File.AppendAllText(path, s[i, j] + " ");
                    }
                }
                File.AppendAllText(path, "|");
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string path = @"C:\Users\Marko\Desktop\sheet.txt";
            string s1 = File.ReadAllText(path);
            int j = 0;
            int k = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i].Equals('['))
                {
                    data[j][k] = "";
                    k++;
                }
                else if (s1[i].Equals('|'))
                {
                    k = 0;
                    j++;
                    continue;
                }
                else
                {
                    string el = "";
                    while (!s1[i].Equals(' '))
                    {
                        el += s1[i];
                        i++;
                    }
                    data[j][k] = el;
                    k++;
                }
            }
        }

        private void dg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
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
    }
}
