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
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : Page
    {
        private ObservableCollection<Customer> _customers = new();
        private ICollectionView _customerView;

        private TextBox _searchBox;
        private DataGrid _customerGrid;

        public Customers()
        {
            InitializeComponent();
            _searchBox = (TextBox)FindName("SearchBox");
            _customerGrid = (DataGrid)FindName("CustomerGrid");
            LoadCustomers();
            _customerView = CollectionViewSource.GetDefaultView(_customers);
            _customerGrid.ItemsSource = _customerView;
        }

        private async void LoadCustomers()
        {
            _customers.Clear();
            try
            {
                using var db = new TobaccoManager.Contexts.AppDbContext();
                var dbCustomers = await Task.Run(() => db.Customers.ToList());
                foreach (var c in dbCustomers)
                    _customers.Add(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load customers: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = _searchBox.Text?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(search))
            {
                _customerView.Filter = null;
            }
            else
            {
                _customerView.Filter = obj =>
                {
                    if (obj is Customer c)
                    {
                        return (c.Name?.ToLower().Contains(search) == true)
                            || (c.Phone?.ToLower().Contains(search) == true)
                            || (c.Address?.ToLower().Contains(search) == true);
                    }
                    return false;
                };
            }
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Not implemented (per instructions)
            MessageBox.Show("Add Customer is not implemented.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Customer customer)
            {
                EditCustomer(customer);
            }
        }

        private async void EditCustomer(Customer customer)
        {
            // Prompt for all editable fields, enforcing model constraints
            string name = Microsoft.VisualBasic.Interaction.InputBox($"Edit Name for {customer.Name}", "Edit Customer", customer.Name);
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string phone = Microsoft.VisualBasic.Interaction.InputBox($"Edit Phone for {customer.Name}", "Edit Customer", customer.Phone ?? "");
            string address = Microsoft.VisualBasic.Interaction.InputBox($"Edit Address for {customer.Name}", "Edit Customer", customer.Address ?? "");

            if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("At least one of phone or address must be provided.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using var db = new TobaccoManager.Contexts.AppDbContext();
                var dbCustomer = db.Customers.FirstOrDefault(c => c.Id == customer.Id);
                if (dbCustomer == null)
                {
                    MessageBox.Show("Customer not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                dbCustomer.Name = name;
                dbCustomer.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone;
                dbCustomer.Address = string.IsNullOrWhiteSpace(address) ? null : address;
                await db.SaveChangesAsync();

                // Update in-memory object
                customer.Name = dbCustomer.Name;
                customer.Phone = dbCustomer.Phone;
                customer.Address = dbCustomer.Address;
                _customerView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update customer: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Customer customer)
            {
                var result = MessageBox.Show($"Delete customer '{customer.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using var db = new TobaccoManager.Contexts.AppDbContext();
                        var dbCustomer = db.Customers.FirstOrDefault(c => c.Id == customer.Id);
                        if (dbCustomer != null)
                        {
                            db.Customers.Remove(dbCustomer);
                            await db.SaveChangesAsync();
                            _customers.Remove(customer);
                        }
                        else
                        {
                            MessageBox.Show("Customer not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete customer: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CustomerGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_customerGrid.SelectedItem is Customer customer)
            {
                EditCustomer(customer);
            }
        }
    }
}
