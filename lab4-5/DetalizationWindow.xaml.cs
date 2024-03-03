﻿using lab4_5.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
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
        private Product _selectedProduct;

        public DetalizationWindow(Product selectedProduct)
        {
            InitializeComponent();
            LoadProductsFromFile("product_data.json");
            FillForm(selectedProduct);
            _selectedProduct = selectedProduct;
        }

        private void bGoBackToAllProducts_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void LoadProductsFromFile(string fileName)
        {
            var pathToFile = Path.Combine(Environment.CurrentDirectory, fileName);

            if (File.Exists(pathToFile))
            {
                var jsonStringProduct = File.ReadAllText(pathToFile);
                _productCollection = JsonSerializer.Deserialize<List<Product>>(jsonStringProduct);
            }
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

        private void bDeleteProduct(object sender, RoutedEventArgs e)
        {
            Delete(_selectedProduct);
            Close();
        }

        private void Delete(Product product)
        {
            if (MessageBox.Show("Вы точно хотите удалить этот продукт?", "Удаление",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _productCollection.Remove(_productCollection.Find(p => p.NameShort == product.NameShort));

                var jsonStringProduct = JsonSerializer.Serialize(_productCollection);
                File.WriteAllText("product_data.json", jsonStringProduct);
            }

            //AllProductWindow newWindow = new AllProductWindow();
            //Application.Current.MainWindow = newWindow;
            //newWindow.Show();
            //this.Close();
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

                image.Source = bitmapImage;
                product.PathToPhoto = imagePath;
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var productToDelete = _productCollection.FirstOrDefault(p => p.NameLong == tbNameLong.Text);

            if (productToDelete != null)
            {
                _productCollection.Remove(productToDelete);
            }

            var product = new Product();
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

            var jsonStringProduct = JsonSerializer.Serialize(_productCollection);
            var pathToFile = Path.Combine(Environment.CurrentDirectory, "product_data.json");
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
    }
}