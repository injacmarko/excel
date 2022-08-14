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

namespace Spreadsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataTable dt = new DataTable();
        public ObservableCollection<DataRow> data = new ObservableCollection<DataRow>();
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
            string[,] s = new string[data.Count, 50];
            for (int i = 0; i < s.GetLength(0); i++)
            {
                for (int j = 0; j < s.GetLength(1); j++)
                {
                    s[i, j] = data[i][j].ToString();
                    File.WriteAllText(path, s[i, j]);
                }
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string path = @"C:\Users\Marko\Desktop\sheet.json";
            ObservableCollection<DataRow> data1 = JsonSerializer.Deserialize<ObservableCollection<DataRow>>(File.ReadAllText(path))!;
            data = data1;
        }
    }
}
