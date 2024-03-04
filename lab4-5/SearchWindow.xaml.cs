using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private Cursor cursor = new Cursor(Application.GetRemoteStream(new Uri("Cursors/myCursor.cur", UriKind.Relative)).Stream);
        public SearchWindow()
        {
            InitializeComponent();
            SearchData = new SearchData();
            search.Cursor = cursor;
        }
        public SearchData SearchData { get; set; }

        private void CommandSearchByname_Click(object sender, ExecutedRoutedEventArgs e)
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

        private void CommandGoBack_Click(object sender, ExecutedRoutedEventArgs e)
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


