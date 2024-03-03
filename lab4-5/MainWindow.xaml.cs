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

namespace lab4_5
{
    public partial class AllProductWindow : Window
    {
        public List<Product> ProductCollection { get; set; }

        private List<Product> _productCollection;
        public AllProductWindow()
        {
            InitializeComponent();

            DataContext = this;

            LoadProductsFromFile("product_data.json");          
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

            _productCollection = _productCollection
                .Where(p => string.IsNullOrEmpty(nameShortSearcCriteria) || p.NameShort.Contains(nameShortSearcCriteria))
                .Where(p => string.IsNullOrEmpty(nameLongSearcCriteria) || p.NameLong.Contains(nameLongSearcCriteria))
                .ToList();
            DataContext = this;
        }

        private void menuAllProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы уже на этой странице", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void menuSelection_Click(object sender, RoutedEventArgs e)
        {
            var selectionWindow = new SelectionWindow();
            selectionWindow.ShowDialog();
        }

        private void menuFilter_Click(object sender, RoutedEventArgs e)
        {
            var filtrationWindow = new FiltrationWindow();
            filtrationWindow.ShowDialog();
        }
    }
}