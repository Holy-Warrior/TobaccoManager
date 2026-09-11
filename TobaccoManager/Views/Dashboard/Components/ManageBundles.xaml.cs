using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.EntityFrameworkCore;
using TobaccoManager.Contexts;
using TobaccoManager.Models;

namespace TobaccoManager.Views.Dashboard.Components
{
    /// <summary>
    /// Lets the user add and remove Bundles for a given Stock.
    /// Changes made here are saved immediately, so the caller should
    /// refresh its own view of the Stock after this window closes.
    /// </summary>
    public partial class ManageBundles : Window
    {
        private readonly int _stockId;
        private readonly ObservableCollection<Bundle> _bundles = new();

        public ManageBundles(int stockId)
        {
            InitializeComponent();
            _stockId = stockId;

            GradeComboBox.ItemsSource = Enum.GetValues(typeof(Grade));
            GradeComboBox.SelectedIndex = 0;

            BundleGrid.ItemsSource = _bundles;
            LoadBundles();
        }

        private void LoadBundles()
        {
            _bundles.Clear();
            try
            {
                using var db = new AppDbContext();
                var stock = db.Stocks.Include(s => s.Bundles).FirstOrDefault(s => s.Id == _stockId);
                if (stock == null)
                {
                    MessageBox.Show("Stock not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }

                TitleBlock.Text = $"Manage Bundles - Stock #{stock.Id}";
                foreach (var bundle in stock.Bundles)
                    _bundles.Add(bundle);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load bundles: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBundle_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(WeightBox.Text.Trim(), out decimal weight) || weight <= 0)
            {
                MessageBox.Show("Enter a valid positive weight.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!decimal.TryParse(PricePerKgBox.Text.Trim(), out decimal pricePerKg) || pricePerKg <= 0)
            {
                MessageBox.Show("Enter a valid positive price per kg.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (GradeComboBox.SelectedItem is not Grade grade)
            {
                MessageBox.Show("Select a leaf grade.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var db = new AppDbContext();
                var stock = db.Stocks.Include(s => s.Bundles).FirstOrDefault(s => s.Id == _stockId);
                if (stock == null)
                {
                    MessageBox.Show("Stock not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var bundle = new Bundle(weight, pricePerKg, grade);
                stock.Bundles.Add(bundle);
                db.SaveChanges();

                _bundles.Add(bundle);

                WeightBox.Clear();
                PricePerKgBox.Clear();
                GradeComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add bundle: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBundle_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.DataContext is not Bundle bundle)
                return;

            var result = MessageBox.Show($"Delete this {bundle.Weight}kg bundle?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                using var db = new AppDbContext();
                var dbBundle = db.Bundles.FirstOrDefault(b => b.Id == bundle.Id);
                if (dbBundle != null)
                {
                    db.Bundles.Remove(dbBundle);
                    db.SaveChanges();
                    _bundles.Remove(bundle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete bundle: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
