using System;
using System.Windows;
using System.Windows.Input;

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        private Cursor cursor = new Cursor(Application.GetRemoteStream(new Uri("Cursors/myCursor.cur", UriKind.Relative)).Stream);
        public SelectionWindow()
        {
            InitializeComponent();
            SelectionData = new SelectionData();
            selection.Cursor = cursor;
        }

        public SelectionData SelectionData { get; set; }

        private void CommandSorting_Click(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionData.Category = cbCategory.Text;
            SelectionData.PriceRange = tbPrice.Text;

            Close();
        }

        private void CommandClose_Click(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }

    public class SelectionData
    {
        public string Category { get; set; }
        public string PriceRange { get; set; }
    }
}
