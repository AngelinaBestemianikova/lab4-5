using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
            SearchData = new SearchData();
        }
        public SearchData SearchData { get; set; }

        private void bSearchByname_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(tbNameShort.Text, @"\d"))
            {
                MessageBox.Show("Поле 'Короткое название' должно содеражать только буквы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SearchData.SearchNameShort = tbNameShort.Text;

            if (Regex.IsMatch(tbNameLong.Text, @"\d"))
            {
                MessageBox.Show("Поле 'Полное название' должно содеражать только буквы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SearchData.SearchNameLong = tbNameLong.Text;

            Close();
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class SearchData
    {
        public string SearchNameShort { get; set; }
        public string SearchNameLong { get; set; }
    }
}


