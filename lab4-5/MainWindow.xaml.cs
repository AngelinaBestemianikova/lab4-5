using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;
using lab4_5.Models;
using System.Windows.Input;
using System.Text.RegularExpressions;



namespace lab4_5
{
    public partial class AllProductWindow : Window
    {
        private Cursor cursor = new Cursor(Application.GetRemoteStream(new Uri("Cursors/myCursor.cur", UriKind.Relative)).Stream);

        public List<Product> ProductCollection { get; set; }

        public AllProductWindow()
        {
            InitializeComponent();

            DataContext = this;

            LoadProductsFromFile("product_data.json");
            
            mainForm.Cursor = cursor;
            mainForm.Left = 100;
            mainForm.Top = 100;
        }

        public void LoadProductsFromFile(string fileName)
        {
            var pathToFile = Path.Combine(Environment.CurrentDirectory, fileName);
            var streamReader = new StreamReader(pathToFile);
            var jsonStringProduct = File.ReadAllText(pathToFile);
            streamReader.Close();

            ProductCollection = JsonSerializer.Deserialize<List<Product>>(jsonStringProduct);
        }

        private void CommandAddProduct_Click(object sender, ExecutedRoutedEventArgs e)
        {
            AddProductWindow addProduct = new AddProductWindow(ProductCollection);
            addProduct.ShowDialog();
            LoadProductsFromFile("product_data.json");
            productsGrid.ItemsSource = ProductCollection;
        }

        private void DataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is Product selectedProduct)
                {
                    DetalizationWindow detalizationWindow = new DetalizationWindow(selectedProduct);
                    detalizationWindow.ShowDialog();
                    LoadProductsFromFile("product_data.json");
                    productsGrid.ItemsSource = ProductCollection;
                }
            }
        }

        private void menuSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchWindow();
            searchWindow.ShowDialog();

            var nameLongSearcCriteria = searchWindow.SearchData.SearchNameLong;
            var nameShortSearcCriteria = searchWindow.SearchData.SearchNameShort;

            ProductCollection = ProductCollection
                .Where(p => string.IsNullOrEmpty(nameShortSearcCriteria) || p.NameShort.Contains(nameShortSearcCriteria))
                .Where(p => string.IsNullOrEmpty(nameLongSearcCriteria) || p.NameLong.Contains(nameLongSearcCriteria))
                .ToList();
            productsGrid.ItemsSource = ProductCollection;
        }

        private void menuAllProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы уже на этой странице", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void menuSelection_Click(object sender, RoutedEventArgs e)
        {
            var selectionWindow = new SelectionWindow();
            selectionWindow.ShowDialog();

            var category = selectionWindow.SelectionData.Category;
            var priceRange = selectionWindow.SelectionData.PriceRange;

            var minPrice = 0.0;
            var maxPrice = 0.0;

            if (!string.IsNullOrEmpty(priceRange))
            {
                if (Regex.IsMatch(priceRange, @"^(?:[\d]+)(?:\\,\\d+)?-(?:[\d]+)(?:\\,\\d+)?$"))
                {
                    if (!string.IsNullOrEmpty(priceRange))
                    {
                        minPrice = double.Parse(priceRange.Split('-')[0]);
                        maxPrice = double.Parse(priceRange.Split('-')[1]);
                    }
                }
                else
                {
                    MessageBox.Show("Недопустипый формат поля 'диапазон'. Ожидаемый формат: 'число-число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
                      
            ProductCollection = ProductCollection
                .Where(p => string.IsNullOrEmpty(category) || p.Category.Contains(category))
                .Where(p => string.IsNullOrEmpty(priceRange) || (p.Price >= minPrice && p.Price <= maxPrice))
                .ToList();
            productsGrid.ItemsSource = ProductCollection;
        }

        private void menuFilter_Click(object sender, RoutedEventArgs e)
        {
            var filtrationWindow = new FiltrationWindow();
            filtrationWindow.ShowDialog();

            var isAvailableCategory = filtrationWindow.FiltrationData.IsAvailable;
            var isNotAvailableCategory = filtrationWindow.FiltrationData.IsNotAvailable;
            var nameLongCategory = filtrationWindow.FiltrationData.NameLong;
            var nameShortCategory = filtrationWindow.FiltrationData.NameShort;
            var priceCategory = filtrationWindow.FiltrationData.Price;
            var countryCategory = filtrationWindow.FiltrationData.Country;
            var category = filtrationWindow.FiltrationData.Category;
            var scoreCategory = filtrationWindow.FiltrationData.Score;
            var quantityCategory = filtrationWindow.FiltrationData.Quantity;
            
            ProductCollection = ProductCollection
                .Where(p => isAvailableCategory == null || (isAvailableCategory == true && p.IsAvailable) || (isAvailableCategory == false && !p.IsAvailable))
                .Where(p => isNotAvailableCategory == null || (isNotAvailableCategory == true && p.IsNotAvailable) || (isNotAvailableCategory == false && !p.IsNotAvailable))
                .Where(p => string.IsNullOrEmpty(category) || p.Category.Contains(category))
                .Where (p => string.IsNullOrEmpty(nameLongCategory) || p.Category.Contains(nameLongCategory))
                .Where (p =>  string.IsNullOrEmpty(nameShortCategory) || p.NameShort.Contains(nameShortCategory))
                .Where (p =>  string.IsNullOrEmpty(countryCategory) || p.Country.Contains(countryCategory))
                .Where (p =>  string.IsNullOrEmpty(priceCategory) || (p.Price >= double.Parse(priceCategory)))
                .Where (p =>  string.IsNullOrEmpty(quantityCategory) || (p.Quantity >= double.Parse(quantityCategory)))
                .Where (p =>  string.IsNullOrEmpty(scoreCategory) || (p.Score >= double.Parse(scoreCategory)))
                .ToList();
            productsGrid.ItemsSource = ProductCollection;
        }

        private void CommandLoadDate_Click(object sender, ExecutedRoutedEventArgs e)
        {
            LoadProductsFromFile("product_data.json");
            productsGrid.ItemsSource = ProductCollection;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 680 && e.NewSize.Width > 400)
            {
                stackPanel.Orientation = Orientation.Horizontal;
                double newLeftMargin = Math.Max(0, e.NewSize.Width - 380);
                bLanguage.Margin = new Thickness(newLeftMargin, 10, 0, 0);
                bLoadimage.Margin = new Thickness(10, 0, 0, 0);
            }
            else if (e.NewSize.Width <= 400)
            {
                stackPanel.Orientation = Orientation.Vertical;
                bLanguage.Margin = new Thickness(0, 10, 0, 0);
                bLoadimage.Margin = new Thickness(0, 10, 0, 0);
            }
            else
            {
                stackPanel.Orientation = Orientation.Horizontal;
                bLanguage.Margin = new Thickness(308, 0, 0, 0);
                bLoadimage.Margin = new Thickness(10, 0, 0, 0);
            }
        }

        private void bLanguage_Click(object sender, RoutedEventArgs e)
        {
            System.Globalization.CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            if (currentCulture.Name == "ru-RU")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            }

            UpdateUiLocalization();
        }

        private void UpdateUiLocalization()
        {
            var newWindow = new AllProductWindow();
            newWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            newWindow.Left = 100;
            newWindow.Top = 100;

            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }
    }
}