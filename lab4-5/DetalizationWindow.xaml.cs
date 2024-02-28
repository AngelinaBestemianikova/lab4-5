using lab4_5.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для DetalizationWindow.xaml
    /// </summary>
    public partial class DetalizationWindow : Window
    {
        private Product product;
        public List<Product> _productCollection;
        public List<Product> ProductCollection { get; set; }
        private Product _selectedProduct;
        public DetalizationWindow(Product selectedProduct)
        {
            InitializeComponent();
            LoadProductsFromFile("product_data.json");
            FillForm(selectedProduct);
            _selectedProduct = selectedProduct;
        }

        private void bGoBackToAllProducts_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoadProductsFromFile(string fileName)
        {
            var pathToFile = Path.Combine(Environment.CurrentDirectory, fileName);
            var streamReader = new StreamReader(pathToFile);
            var jsonStringStudent = File.ReadAllText(pathToFile);
            streamReader.Close();

            ProductCollection = JsonSerializer.Deserialize<List<Product>>(jsonStringStudent);
        }

        private void FillForm(Product product)
        {
            tbNameShort.Text = product.NameShort;
            tbNameLong.Text = product.NameLong;
            cbCategory.Text = product.Category;
            tbPrice.Text = product.Price.ToString();
            tbQuantity.Text = product.Quantity.ToString();
            tbDescription.Text = product.Description;
            tbCountry.Text = product.Country;
            tbScore.Text = product.Score.ToString();
            rbNone.IsChecked = product.IsNotAvailable;
            rbYes.IsChecked = product.IsAvailable;

            if (!string.IsNullOrEmpty(product.PathToPhoto))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(product.PathToPhoto);
                bitmap.EndInit();

                image.Source = bitmap;
            }
        }

        private void Delete(Product product)
        {
            if (MessageBox.Show("Вы точно хотите удалить этот продукт?", "Удаление",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _productCollection.Remove(product);
            }           
        }

        private void bDeleteProduct(object sender, RoutedEventArgs e)
        {
            Delete(_selectedProduct);
            LoadProductsFromFile("product_data.json");
            Close();
        }

        private void bEditProduct_Click(object sender, RoutedEventArgs e)
        {
            tbCountry.IsReadOnly = false;
            tbScore.IsReadOnly = false;
            tbDescription.IsReadOnly = false;
            tbNameLong.IsReadOnly = false;
            tbNameShort.IsReadOnly = false;
            tbPrice.IsReadOnly = false;
            tbQuantity.IsReadOnly = false;
            cbCategory.IsEnabled = true;
            rbNone.IsEnabled = true;
            rbYes.IsEnabled = true;
            LoadImage.IsEnabled = true;
            SaveChanges.IsEnabled = true;
        }

        private void bLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath);
                bitmapImage.EndInit();

                // Присвойте bitmapImage свойству Image на вашем окне или элементе управления
                // Например, если у вас есть элемент Image с именем "imageControl":
                image.Source = bitmapImage;

                product.PathToPhoto = imagePath;
            }
        }
    }
}