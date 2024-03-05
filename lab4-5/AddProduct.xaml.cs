using lab4_5.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
        private Cursor cursor = new Cursor(Application.GetRemoteStream(new Uri("Cursors/myCursor.cur", UriKind.Relative)).Stream);

        private Product product;
        private string pathToFile;

        public AddProductWindow(List<Product> productCollection)
        {

            InitializeComponent();
            SizeChanged += Window_SizeChanged;
            _productCollection = productCollection;
            product = new Product();
            pathToFile = Path.Combine(Environment.CurrentDirectory, "product_data.json");
            add.Cursor = cursor;
            add.Left = 30;
            add.Top = 30;

            if (File.Exists(pathToFile))
            {
                var jsonString = File.ReadAllText(pathToFile);
                _productCollection = JsonSerializer.Deserialize<List<Product>>(jsonString);
            }
            else
            {
                _productCollection = new List<Product>();
            }
        }

        private void CommandSaveToFile_Click(object sender, ExecutedRoutedEventArgs e)
        {
            product.NameShort = tbNameShort.Text;
            product.NameLong = tbNameLong.Text;
            product.Description = tbDescription.Text;
            product.Category = cbCategory.Text;

            if (string.IsNullOrEmpty(tbQuantity.Text))
            {
                MessageBox.Show("Поле 'Количесто' пустое. Введите количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(tbQuantity.Text, @"\D"))
            {
                MessageBox.Show("Поле 'Количество' должно содеражать только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            product.Quantity = double.Parse(tbQuantity.Text);

            if (string.IsNullOrEmpty(tbPrice.Text))
            {
                MessageBox.Show("Поле 'Стоимость' пустое. Введите стоимость", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(tbPrice.Text, @"\d+(?:\,\d+)?"))
            {
                product.Price = double.Parse(tbPrice.Text);
            }
            else
            {

                MessageBox.Show("Поле 'Стоимость' должно содеражать только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


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

            if (Regex.IsMatch(tbCountry.Text, @"\d"))
            {
                MessageBox.Show("Поле 'Страна' должно содеражать только буквы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            product.Country = tbCountry.Text;

            if (string.IsNullOrEmpty(tbScore.Text))
            {
                MessageBox.Show("Поле 'Рейтинг' пустое. Введите рейтинг", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(tbScore.Text, @"\d+(?:\,\d+)?"))
            {
                product.Score = double.Parse(tbScore.Text);

            }
            else
            {
                MessageBox.Show("Поле 'Рейтинг' должно содеражать только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var validationContext = new ValidationContext(product);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(product, validationContext, results, true))
            {
                MessageBox.Show(results.First().ErrorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _productCollection.Add(product);

            var jsonStringProduct = JsonSerializer.Serialize(_productCollection);
            File.WriteAllText(pathToFile, jsonStringProduct);
            MessageBox.Show($"Файл успешно сохранен.\nПуть: {pathToFile}", "Сохранение в файл", MessageBoxButton.OK, MessageBoxImage.Information);

            //tbNameShort.Text = null;
            //tbNameLong.Text = null;
            //tbDescription.Text = null;
            //cbCategory.Text = null;
            //tbCountry.Text = null;
            //tbQuantity.Text = null;
            //tbPrice.Text = null;
            //tbScore.Text = null;
            //rbNone.IsChecked = false;
            //rbYes.IsChecked = false;
            //image.Source = null;
        }

        private void bGoBackToAllProducts_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CommandLoadImage_Click(object sender, ExecutedRoutedEventArgs e)
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

                image.Source = bitmapImage;

                product.PathToPhoto = imagePath;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 680)
            {
                add.Height = 1600;
                grid1.Margin = new Thickness(-220, 350, 0, 0);
                
                double newLeftMargin = Math.Max(0, e.NewSize.Width - 280);
                bGoBackToAllProducts.Margin = new Thickness(newLeftMargin, 340, 0, 0);
            }
            else
            {
                add.Height = 500;
                grid1.Margin = new Thickness(0, 0, 0, 0);
                grid2.Margin = new Thickness(0, 0, 0, 0);
                bGoBackToAllProducts.Margin = new Thickness(0, 340, 0, 0);
            }
        }

    }
}