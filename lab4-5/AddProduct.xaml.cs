using lab4_5.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public List<Product> _productCollection;
        
        private Product product;
        private string pathToFile;
       
        public AddProductWindow(List<Product> productCollection)
        {
            InitializeComponent();
            _productCollection = productCollection;
            product = new Product();         
            pathToFile = Path.Combine(Environment.CurrentDirectory, "product_data.json");
        }

        private void bSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            product.NameShort = tbNameShort.Text;            
            product.NameLong = tbNameLong.Text;
            product.Description = tbDescription.Text;
            product.Category = cbCategory.Text;
            product.Country = tbCountry.Text;

            var validationContext = new ValidationContext(product);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(product, validationContext, results, true))
            {
                MessageBox.Show(results.First().ErrorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(tbQuantity.Text))
            {
                MessageBox.Show("Поле 'Количесто' пустое. Введите количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            product.Quantity = double.Parse(tbQuantity.Text);                      

            if (string.IsNullOrEmpty(tbPrice.Text))
            {
                MessageBox.Show("Поле 'Стоимость' пустое. Введите стоимость", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            product.Price = decimal.Parse(tbPrice.Text);
      
            if (rbYes.IsChecked == false && rbNone.IsChecked == false)
            {
                MessageBox.Show("Выберите доступность товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if ((bool)rbYes.IsChecked)
            {
                product.IsAvailable = true;
            }
            else
            {
                product.IsNotAvailable = true;
            }

            if (string.IsNullOrEmpty(tbScore.Text))
            {
                MessageBox.Show("Поле 'Рейтинг' пустое. Введите рейтинг", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            product.Score = double.Parse(tbScore.Text);           

            _productCollection.Add(product);

            var jsonStringStudent = JsonSerializer.Serialize(_productCollection);
            var streamWriter = new StreamWriter(pathToFile);
            streamWriter.Write(jsonStringStudent);
            streamWriter.Flush();
            streamWriter.Close();
        }

        private void bGoBackToAllProducts_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
