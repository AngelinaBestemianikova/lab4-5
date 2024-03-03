using System.Windows;

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для FiltrationWindow.xaml
    /// </summary>
    public partial class FiltrationWindow : Window
    {
        public FiltrationWindow()
        {
            InitializeComponent();
            FiltrationData = new FiltrationData();
        }

        public FiltrationData FiltrationData { get; set; }

        private void bGoBackToAllProducts_Click(object sender, RoutedEventArgs e)
        {
            FiltrationData.NameShort = tbNameShort.Text;
            FiltrationData.NameLong = tbNameLong.Text;
            FiltrationData.Category = cbCategory.Text;
            FiltrationData.Price = tbPrice.Text;
            FiltrationData.Quantity = tbQuantity.Text;
            FiltrationData.Score = tbScore.Text;
            FiltrationData.Country = tbCountry.Text;
            FiltrationData.IsAvailable = rbYes.IsChecked;
            FiltrationData.IsNotAvailable = rbNone.IsChecked;

            Close();
        }
    }

    public class FiltrationData
    {
        public string NameShort { get; set; }

        public string NameLong { get; set; }

        public string Category { get; set; }

        public string Price { get; set; }

        public string Quantity { get; set; }

        public string Score { get; set; }

        public string Country { get; set; }

        public bool? IsAvailable { get; set; }

        public bool? IsNotAvailable { get; set; }
    }
}
