using System.Windows;
using System.Windows.Input;

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        public SelectionWindow()
        {
            InitializeComponent();
            SelectionData = new SelectionData();
        }

        public SelectionData SelectionData { get; set; }

        private void CommandClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionData.Category = cbCategory.Text;
            SelectionData.PriceRange = tbPrice.Text;

            Close();
        }
    }

    public class SelectionData
    {
        public string Category { get; set; }
        public string PriceRange { get; set; }
    }
}
