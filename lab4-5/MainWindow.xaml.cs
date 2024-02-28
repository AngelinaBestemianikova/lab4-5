using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
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

        private void LoadProductsFromFile(string fileName)
        {
            var pathToFile = Path.Combine(Environment.CurrentDirectory, fileName);
            var streamReader = new StreamReader(pathToFile);
            var jsonStringStudent = File.ReadAllText(pathToFile);
            streamReader.Close();                    

            ProductCollection = JsonSerializer.Deserialize<List<Product>>(jsonStringStudent);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProduct = new AddProductWindow(_productCollection);
            addProduct.ShowDialog();
        }

        private void DataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is Product selectedProduct)
                {
                    DetalizationWindow detalizationWindow = new DetalizationWindow(selectedProduct);
                    detalizationWindow.ShowDialog();
                }
            }
        }
    }
}
