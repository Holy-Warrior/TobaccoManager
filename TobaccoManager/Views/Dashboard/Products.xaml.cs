using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TobaccoManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TobaccoManager.Views.Dashboard
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Page
    {
        private ObservableCollection<Stock> _stocks = new();
        private ICollectionView _stockView;
        private TextBox _searchBox;
        private DataGrid _stockGrid;

        public Products()
        {
            InitializeComponent();
            _searchBox = (TextBox)FindName("SearchBox");
            _stockGrid = (DataGrid)FindName("StockGrid");
            LoadStocks();
            _stockView = CollectionViewSource.GetDefaultView(_stocks);
            _stockGrid.ItemsSource = _stockView;
        }

        private async void LoadStocks()
        {
            _stocks.Clear();
            try
            {
                using var db = new TobaccoManager.Contexts.AppDbContext();
                var dbStocks = await Task.Run(() => db.Stocks.ToList());
                foreach (var s in dbStocks)
                    _stocks.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load stocks: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = _searchBox.Text?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(search))
            {
                _stockView.Filter = null;
            }
            else
            {
                _stockView.Filter = obj =>
                {
                    if (obj is Stock s)
                    {
                        return s.Price.ToString().Contains(search)
                            || s.DateReceived.ToString().ToLower().Contains(search)
                            || (s.DatePaid?.ToString().ToLower().Contains(search) == true)
                            || s.NoOfBundles.ToString().Contains(search)
                            || s.GrandTotalPrice.ToString().Contains(search);
                    }
                    return false;
                };
            }
        }

        private async void AddStockButton_Click(object sender, RoutedEventArgs e)
        {
            var addStockWindow = new Components.AddProducts();
            if (addStockWindow.ShowDialog() == true)
            {
                var newStock = addStockWindow.NewStock;
                try
                {
                    using var db = new TobaccoManager.Contexts.AppDbContext();
                    db.Stocks.Add(newStock);
                    await db.SaveChangesAsync();
                    _stocks.Add(newStock);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save new stock: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditStock_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Stock stock)
            {
                EditStock(stock);
            }
        }

        private async void EditStock(Stock stock)
        {
            // Prompt for editable fields
            string priceStr = Microsoft.VisualBasic.Interaction.InputBox($"Edit Price for Stock #{stock.Id}", "Edit Stock", stock.Price.ToString());
            if (!decimal.TryParse(priceStr, out decimal price))
            {
                MessageBox.Show("Invalid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string dateReceivedStr = Microsoft.VisualBasic.Interaction.InputBox($"Edit Date Received (yyyy-MM-dd) for Stock #{stock.Id}", "Edit Stock", stock.DateReceived.ToString("yyyy-MM-dd"));
            if (!DateOnly.TryParse(dateReceivedStr, out DateOnly dateReceived))
            {
                MessageBox.Show("Invalid date received.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string datePaidStr = Microsoft.VisualBasic.Interaction.InputBox($"Edit Date Paid (yyyy-MM-dd, optional) for Stock #{stock.Id}", "Edit Stock", stock.DatePaid?.ToString("yyyy-MM-dd") ?? "");
            DateOnly? datePaid = null;
            if (!string.IsNullOrWhiteSpace(datePaidStr))
            {
                if (!DateOnly.TryParse(datePaidStr, out var dp))
                {
                    MessageBox.Show("Invalid date paid.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                datePaid = dp;
            }

            try
            {
                using var db = new TobaccoManager.Contexts.AppDbContext();
                var dbStock = db.Stocks.FirstOrDefault(s => s.Id == stock.Id);
                if (dbStock == null)
                {
                    MessageBox.Show("Stock not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                dbStock.Price = price;
                dbStock.DateReceived = dateReceived;
                dbStock.DatePaid = datePaid;
                await db.SaveChangesAsync();

                // Update in-memory object
                stock.Price = dbStock.Price;
                stock.DateReceived = dbStock.DateReceived;
                stock.DatePaid = dbStock.DatePaid;
                _stockView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update stock: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteStock_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Stock stock)
            {
                var result = MessageBox.Show($"Delete stock #{stock.Id}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using var db = new TobaccoManager.Contexts.AppDbContext();
                        var dbStock = db.Stocks.FirstOrDefault(s => s.Id == stock.Id);
                        if (dbStock != null)
                        {
                            db.Stocks.Remove(dbStock);
                            await db.SaveChangesAsync();
                            _stocks.Remove(stock);
                        }
                        else
                        {
                            MessageBox.Show("Stock not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete stock: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void StockGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_stockGrid.SelectedItem is Stock stock)
            {
                EditStock(stock);
            }
        }
    }
}
